using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental.Model
{
    public class RentalDataContext: DbContext
    {

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Rental> Rentals{ get; set; }
        public RentalDataContext(DbContextOptions<RentalDataContext> options)
        : base(options)
        { }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Customer>().HasIndex(b => b.Id).IsUnique();

        }
      /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=CookieRecipe;User Id=sa;Password = Password1!;");
        }*/
    }
}
