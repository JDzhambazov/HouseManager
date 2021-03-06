namespace HouseManager.Services.Data
{
    using System;

    using HouseManager.Data.Models;
    using HouseManager.Services.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IExpensService
    {
        void AddExpens(string expensType, decimal price, DateTime date, bool isRegular, int addressId);

        void EditExpense(int expenseId, int expenseTypeId, decimal newPrice, DateTime date, int addressId);

        void DeleteExpense(int expenseId, int addressId);

        PagingServiceModel<AllExpenseServiseModel> GetAll(int addressId, int page, DateTime startDate, DateTime endDate);

        Expens GetById(int expenseId, int addressId);

        SelectList GetExpenseTypes(int addressId);
    }
}
