namespace HouseManager.Web.ViewModels.Incomes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using HouseManager.Data.Models;

    public class IncomeViewModel
    {
        public int Id { get; set; }

        public int? PropertyId { get; set; }

        public Property Property { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public string ResidentId { get; set; }

        public ApplicationUser Resident { get; set; }
    }
}
