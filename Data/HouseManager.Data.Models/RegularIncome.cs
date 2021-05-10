namespace HouseManager.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RegularIncome
    {
        public int Id { get; set; }

        public int? PropertyId { get; set; }

        public Property Property { get; set; }

        [MaxLength(6)]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public string ResidentId { get; set; }

        public ApplicationUser Resident { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }
    }
}
