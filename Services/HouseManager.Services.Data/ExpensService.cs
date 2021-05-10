namespace Services
{
    using System;
    using System.Linq;

    using HouseManager.Data;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data;

    public class ExpensService : IExpensService
    {
        private readonly ApplicationDbContext db;

        public ExpensService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddExpens(string expensType, decimal price, DateTime date, bool isRegular, int addressId)
        {
            this.db.Expens.Add(new Expens
            {
                ExpenseType = this.db.ExpensesTypes.FirstOrDefault(x => x.Name == expensType)
                ?? new ExpensType { Name = expensType },
                Price = price,
                DateOfPayment = date,
                AddressId = addressId,
                IsRegular = isRegular,
            });
            this.db.SaveChanges();
        }
    }
}
