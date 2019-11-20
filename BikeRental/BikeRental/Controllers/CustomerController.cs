using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeRental.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BikeRental.Controllers
{
    [ApiController]
    [Route("/api")]
    public class CustomerController : ControllerBase
    {
        RentalDataContext Context;

        public CustomerController(RentalDataContext context)
        {
            Context = context;
        }
       
        [HttpGet]
        [Route("customers")]
        public IActionResult GetCustomers([FromQuery]string NameFilter)
        {

            if (NameFilter != null)
            {
                IEnumerable<Customer> c = (Context.Customers.Where(i => i.LastName.ToLower().Contains(NameFilter.ToLower())));
                if (c.Count() > 0)
                {
                    return Ok(c);
                }
                return NotFound();
            }
            return Ok(Context.Customers);
        }
        [HttpPost]
        [Route("customers")]

        public async Task<IActionResult> AddCustomer([FromBody] Customer NewCustomer)
        {
            Context.Customers.Add(NewCustomer);
            await Context.SaveChangesAsync();
                return Created("",NewCustomer);

        }
        [HttpPut]
        [Route("customers/{index}")]
        public async Task<IActionResult> UpdateContact(int index, [FromBody]  Customer NewCustomer)
        {
            try
            {
                using var transaction = Context.Database.BeginTransaction();
                NewCustomer.Id = index;
                var c = await Context.Customers.FirstOrDefaultAsync(elem => elem.Id == index);
                Context.Customers.Remove(c);
                Context.Customers.Add(NewCustomer);
                await Context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(NewCustomer);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Something bad happened: {ex.Message}");

                throw;
               
            }
        }
        [HttpDelete]
        [Route("customers/{index}")]
        public async Task<IActionResult> DeleteContact(int index)
        {
            
            var c = await Context.Customers.FirstOrDefaultAsync(elem => elem.Id == index);
            if (c == null) return NotFound($"not found: contact with id {index}");
            Context.Customers.Remove(c);
            await Context.SaveChangesAsync();
            return Ok($"Deleted contact with id {index}");
            
        }
        [HttpGet]
        [Route("customers/{index}/rentals")]
        public async Task<IActionResult> GetAllRentals(int index)
        {

            var rentals = Context.Rentals.Where(r=> r.CustomerID==index);
            var result = await rentals.ToArrayAsync();
            if (result.Length > 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound($"no rentals found for contact with id {index}");
            }
        }

    }
}
