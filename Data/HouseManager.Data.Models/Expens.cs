namespace HouseManager.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Expens
    {
        public int Id { get; set; }

        public int ExpensTypeId { get; set; }

        public ExpensType ExpenseType { get; set; }

        [MaxLength(10)]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public DateTime DateOfPayment { get; set; }

        public bool IsRegular { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }
    }
}
