namespace HouseManager.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AllExpenseServiseModel
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public string ExpenseName { get; set; }
    }
}
