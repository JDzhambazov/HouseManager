namespace HouseManager.Services.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using HouseManager.Data.Models;
    using HouseManager.Services.Mapping;

    public class IncomeServiceModel : IMapFrom<RegularIncome>, IMapFrom<NotRegularIncome>
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
