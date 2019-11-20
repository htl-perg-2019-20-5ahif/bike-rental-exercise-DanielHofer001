using BikeRental;
using BikeRental.Controllers;
using System;
using Xunit;

namespace Tests.BikeRental
{
    public class UnitTest1
    {
        /*
         * ental begin	14th Feb. 2018, 08:15am
Rental end	14th Feb. 2018, 10:30am
Bike price first hour	3 Euro
Bike price each additional hour	5 Euro
Total costs	3 Euro for first hour (08:15am - 09:15am)
5 Euro for additional hour (09:15am - 10:15am)
5 Euro for additional hour (10:15am - 10:30am)
= 13 Euro in total
Property	Value
Rental begin	14th Feb. 2018, 08:15am
Rental end	14th Feb. 2018, 08:45am
Bike price first hour	3 Euro
Total costs	3 Euro for first hour
Property	Value
Rental begin	14th Feb. 2018, 08:15am
Rental end	14th Feb. 2018, 08:25am
Total costs	Free because <= 15 minutes

    */
        [Fact]
        public void CalculateAdditionalHour()
        {

            Bike bike = new Bike
            {
                RentalPriceFirstHour = 3,
                RentalPriceAddHour = 5,
                Id = 1
            };
            Customer customer = new Customer
            {
                Id = 1
            };
            Rental r = new Rental
            {
                Begin = new DateTime(2018, 02, 14, 8, 15, 0),
                End = new DateTime(2018, 02, 14, 10, 30, 0),
                Id = 1,
                Customer = customer,
                CustomerID = customer.Id,
                Bike = bike,
                BikeID = bike.Id
            };
            r.TotalCosts = CostCalculation.CalculateTotalCost(r);
            Assert.Equal(13, r.TotalCosts, 1);
        }

        [Fact]
        public void CalculateFirstHour()
        {
            Bike bike = new Bike
            {
                RentalPriceFirstHour = 3,
                RentalPriceAddHour = 5,
                Id = 1
            };
            var customer = new Customer
            {
                Id = 1
            };
            Rental r = new Rental
            {
                Begin = new DateTime(2018, 02, 14, 8, 15, 0),
                End = new DateTime(2018, 02, 14, 8, 45, 0),
                Id = 1,
                Customer = customer,
                CustomerID = customer.Id,
                Bike = bike,
                BikeID = bike.Id
            };
            r.TotalCosts = CostCalculation.CalculateTotalCost(r);
            Assert.Equal(3, r.TotalCosts, 1);
        }

        [Fact]
        public void CalculateFree()
        {
            Bike bike = new Bike
            {
                RentalPriceFirstHour = 3,
                RentalPriceAddHour = 5,
                Id = 1
            };


            Customer customer = new Customer
            {
                Id = 1
            };
            Rental r = new Rental
            {
                Begin = new DateTime(2018, 02, 14, 8, 15, 0),
                End = new DateTime(2018, 02, 14, 8, 25, 0),
                Id = 1,
                Customer = customer,
                CustomerID = customer.Id,
                Bike = bike,
                BikeID = bike.Id
            };
            r.TotalCosts = CostCalculation.CalculateTotalCost(r);
            Assert.Equal(0, r.TotalCosts, 1);
        }
    }
}
