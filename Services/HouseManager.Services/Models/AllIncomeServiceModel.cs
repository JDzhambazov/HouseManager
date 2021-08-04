namespace HouseManager.Services.Models
{
    using System;

    public class AllIncomeServiceModel
    {
        public int Id { get; set; }

        public int? PropertyId { get; set; }

        public string PropertyName { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public string IncomeName { get; set; }

        public string ResidentFullName { get; set; }
    }
}
