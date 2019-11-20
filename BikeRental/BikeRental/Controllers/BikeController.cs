using BikeRental.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BikeController : ControllerBase
    {
        private readonly RentalDataContext Context;

        public BikeController(RentalDataContext context)
        {
            Context = context;
        }

        [HttpGet]
        [Route("bikes")]
        public async Task<ActionResult> GetBikes([FromQuery] string Sort)
        {
            var c = await Context.Bikes.ToListAsync();


            if (Sort != string.Empty)
            {
                if (Sort == "firsthour")
                {
                    c = await Context.Bikes.OrderBy(b => b.RentalPriceFirstHour).ToListAsync();

                }
                else if (Sort == "additionalhours")
                {
                    c = await Context.Bikes.OrderBy(b => b.RentalPriceAddHour).ToListAsync(); ;
                }
                else if (Sort == "purchasedate")
                {
                    c= await Context.Bikes.OrderByDescending(b => b.PurchaseDate).ToListAsync();
                }
            }
            return Ok(c);
        }



        [HttpPut]
        [Route("bikes/{id}")]

        public async Task<IActionResult> PutBike(int id, [FromBody] Bike bike)
        {
            using var transaction = Context.Database.BeginTransaction();
            bike.Id = id;
            var c = await Context.Bikes.FindAsync(id);
            if(c==null) return NotFound($"not found: bike with id {id}");
            Context.Bikes.Remove(c);
            Context.Bikes.Add(bike);
            await Context.SaveChangesAsync();
            await transaction.CommitAsync();
            return Ok(bike);


        }

        [HttpPost]
        [Route("bikes")]

        public async Task<ActionResult> PostBike([FromBody]Bike bike)
        {
            Context.Bikes.Add(bike);
            await Context.SaveChangesAsync();
            return Created("", bike);
        }
        [HttpDelete]
        [Route("bikes/{id}")]

        public async Task<ActionResult> DeleteBike(int id)
        {

            var c = await Context.Bikes.FindAsync(id);
            if (c == null) return NotFound($"not found: bike with id {id}");
            Context.Bikes.Remove(c);
            await Context.SaveChangesAsync();
            return Ok($"Deleted bike with id {id}");

        }


    }
}
