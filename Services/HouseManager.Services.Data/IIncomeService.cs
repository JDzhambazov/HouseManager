namespace HouseManager.Services.Data
{
    using System;
    using System.Collections.Generic;

    using HouseManager.Data.Models;
    using HouseManager.Web.ViewModels.Incomes;

    public interface IIncomeService
    {
        void AddIncome(int? properyId, decimal price, DateTime date, ApplicationUser resident, int addressId, bool isRegular);

        ICollection<IncomeViewModel> GetAllIncomeForPropery(int properyId, bool isRegular);

        void EditMounthProperyIncome(int propertyID, int month, int year, decimal newPrice, bool isRegular);
    }
}
