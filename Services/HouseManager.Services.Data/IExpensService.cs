namespace HouseManager.Services.Data
{
    using System;

    using HouseManager.Data.Models;

    public interface IExpensService
    {
        void AddExpens(string expensType, decimal price, DateTime date, bool isRegular, int addressId);
    }
}
