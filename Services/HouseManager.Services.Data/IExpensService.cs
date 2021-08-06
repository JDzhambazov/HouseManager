namespace HouseManager.Services.Data
{
    using System;

    using HouseManager.Data.Models;
    using HouseManager.Services.Models;

    public interface IExpensService
    {
        void AddExpens(string expensType, decimal price, DateTime date, bool isRegular, int addressId);

        PagingServiceModel<AllExpenseServiseModel> GetAll(int addressId, int page, DateTime startDate, DateTime endDate);
    }
}
