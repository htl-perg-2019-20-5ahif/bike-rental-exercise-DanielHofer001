using BikeRental.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly RentalDataContext Context;

        public RentalController(RentalDataContext context)
        {
            Context = context;
        }

        [HttpPost]
        [Route("rentals/start")]
        public async Task<ActionResult> StartRental([FromBody] Rental rental)
        {
            try
            {
                rental.Customer = await Context.Customers.FindAsync(rental.CustomerID);
                rental.Bike = await Context.Bikes.FindAsync(rental.BikeID);
                rental.Begin = System.DateTime.Now;
                var alreadyRunning=await Context.Rentals.AnyAsync(b => b.CustomerID == rental.CustomerID && b.End == DateTime.MinValue);
                if (alreadyRunning) return BadRequest("alreadyRunning rental");
                Context.Rentals.Add(rental);
                await Context.SaveChangesAsync();
                return Ok(rental);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("rentals/stop")]
        public async Task<ActionResult> EndRental([FromBody] Rental rental)
        {
            try
            {
                var startedRental = await Context.Rentals.FirstAsync(b=> b.CustomerID==rental.CustomerID && b.BikeID==rental.BikeID && b.End==DateTime.MinValue);
                
                startedRental.End = System.DateTime.Now;
                startedRental.TotalCosts = CostCalculation.CalculateTotalCost(startedRental);
                startedRental.Customer = await Context.Customers.FindAsync(rental.CustomerID);
                startedRental.Bike = await Context.Bikes.FindAsync(rental.BikeID);

                Context.Rentals.Update(startedRental);
                //Context.Rentals.Add(startedRental);

                await Context.SaveChangesAsync();
                return Ok(startedRental);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        [HttpPost]
        [Route("rentals/paid")]
        public async Task<ActionResult> MarkAsPaid([FromBody] Rental rental)
        {
            try
            {
                using var transaction = Context.Database.BeginTransaction();

                var endedRental = await Context.Rentals.FirstAsync(b => b.CustomerID == rental.CustomerID && b.BikeID == rental.BikeID && b.End == DateTime.MinValue);
                if (endedRental != null)
                {
                    if (endedRental.TotalCosts == 0)
                    {
                        return BadRequest("Can only be executed on rentals that have a total price > 0");

                    }
                    endedRental.Paid = true;
                }
                Context.Rentals.Update(endedRental);

                await Context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(endedRental);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        [HttpGet]
        [Route("rentals/unpaid")]
        public async Task<ActionResult> GetUnpaid()
        {
            var unpaid= Context.Rentals.Where(b=>b.End!=null && b.TotalCosts>0 && b.Paid!=true);
            var result=await unpaid
                .Include(c => c.Customer)
                .Select(item => new { item.CustomerID, item.Customer.LastName, item.Customer.FirstName, item.Id, item.Begin, item.End })
                .ToListAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("rentals")]
        public async Task<ActionResult> GetAll()
        {
            var result = await Context.Rentals.Include(c => c.Customer)
                .Select(item => new { item.CustomerID, item.Customer.LastName, item.Customer.FirstName,item.Id, item.Begin,item.End}).ToListAsync();
            return Ok(result);
        }
    }
}
