namespace HouseManager.Services.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using HouseManager.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EditIncomeServiceModel
    {
        public int Id { get; set; }

        public int? PropertyId { get; set; }

        public string PropertyName { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string IncomeName { get; set; }

        [Required]
        public string ResidentId { get; set; }

        public int AddressId { get; set; }
    }
}
