namespace HouseManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using HouseManager.Data;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data;
    using HouseManager.Services.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class ExpensService : IExpensService
    {
        private readonly ApplicationDbContext db;
        private readonly IPagingService<AllExpenseServiseModel> pagingService;

        public ExpensService(ApplicationDbContext db,
            IPagingService<AllExpenseServiseModel> pagingService)
        {
            this.db = db;
            this.pagingService = pagingService;
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

        public void EditExpense(int expenseId,int expenseTypeId, decimal newPrice, DateTime date, int addressId)
        {
            var expense = this.db.Expens
                .FirstOrDefault(x => x.Id == expenseId && x.AddressId == addressId);

            if (expense != null)
            {
                expense.Price = newPrice;
                expense.DateOfPayment = date;
                expense.ExpensTypeId = expenseTypeId;
            }

            db.Entry(expense).State = EntityState.Modified;
            this.db.SaveChanges();
        }

        public void DeleteExpense(int expenseId, int addressId)
        {
            var expense = db.Expens
                .FirstOrDefault(x => x.Id == expenseId && x.AddressId == addressId);

            if (expense != null)
            {
                this.db.Expens.Remove(expense);
            }

            this.db.SaveChanges();
        }

        public PagingServiceModel<AllExpenseServiseModel> GetAll(int addressId, int page, DateTime startDate, DateTime endDate)
        {
            var expense = new List<AllExpenseServiseModel>();

            var expenseQuery = db.Expens
                .Where(x => x.AddressId == addressId);

            if (startDate > DateTime.MinValue && startDate < DateTime.Now)
            {
                expenseQuery = expenseQuery
                    .Where(x => x.DateOfPayment >= startDate);

            }

            if (endDate > DateTime.MinValue && endDate < DateTime.Now)
            {
                expenseQuery = expenseQuery
                    .Where(x => x.DateOfPayment <= endDate);
            }

            expense = expenseQuery
                .Select(x => new AllExpenseServiseModel
                {
                    Id = x.Id,
                    ExpenseName = x.ExpenseType.Name,
                    Price = x.Price,
                    Date = x.DateOfPayment,
                })
                .ToList();

            expense = expense.OrderByDescending(x => x.Date).ToList();

            return this.pagingService.GetPageInfo(expense, page);
        }

        public Expens GetById(int expenseId , int addressId)
        => db.Expens.FirstOrDefault(x => x.Id == expenseId && x.AddressId == addressId);

        public SelectList GetExpenseTypes(int addressId)
        {
            var listItems = new List<SelectListItem>();
            var expenses = this.db.ExpensesTypes
                .ToList();

            foreach (var item in expenses)
            {
                listItems.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name,
                });
            }
            return new SelectList(listItems,"Value", "Text", null);
        }
    }
}
