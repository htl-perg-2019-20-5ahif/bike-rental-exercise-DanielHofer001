using System;

namespace BikeRental
{
    public class CostCalculation
    {

        public static float CalculateTotalCost(Rental rental)
        {
            float TotalCosts = 0;
            var dif = rental.End.Subtract(rental.Begin);
            if (dif.TotalMinutes <= 15) TotalCosts = 0;
            else if (dif.TotalMinutes <= 60) TotalCosts = (float)rental.Bike.RentalPriceFirstHour;
            else
            {
                TotalCosts = rental.Bike.RentalPriceFirstHour + (float)(Math.Ceiling(dif.TotalHours - 1) * rental.Bike.RentalPriceAddHour);
            }
            return TotalCosts;
        }

    }
}
