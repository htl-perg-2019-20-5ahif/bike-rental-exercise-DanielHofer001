using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BikeRental
{
    public class Customer 
    {
        [Key]
        public Int32 Id { get; set; }
        public enum Gender { Male, Female, Unknown }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(75)]
        public string LastName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        [MaxLength(75)]
        public string Street { get; set; }
        [MaxLength(10)]
        public string HouseNumber { get; set; }
        [Required]
        [MaxLength(10)]
        public string ZipCode { get; set; }
        [Required]
        [MaxLength(75)]
        public string Town{ get; set; }
        public List <Rental> Bikes{ get; set; }
    }
    public class Bike
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Brand { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; }
        public DateTime DateOfLastService { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        [DataType("decimal(18,2)")]
        public float RentalPriceFirstHour { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        [DataType("decimal(18,2)")]
        public float RentalPriceAddHour { get; set; }
        public enum BikeCategory { Standardbike, Mountainbike, Treckingbike, Racingbike }
        public List<Rental> Customers { get; set; }


    }
    public class Rental
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public int CustomerID { get; set; }

        public Customer Customer { get; set; }
        [Required]

        public int BikeID { get; set; }

        public Bike Bike { get; set; }
        [Required]
        public DateTime Begin { get; set; }

        public DateTime End
             { get; set; }
    
        [Range(0, float.MaxValue)]
        [DataType("decimal(18,2)")]
        public float TotalCosts { get; set; }
        public bool Paid { get; set; }
        
    }

}
