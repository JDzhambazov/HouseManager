namespace HouseManager.Services.Data
{
    using System;
    using System.Collections.Generic;

    using HouseManager.Data.Models;
    using HouseManager.Services.Data.Models;
    using HouseManager.Services.Models;

    public interface IIncomeService
    {
        void AddIncome(int? properyId, decimal price, DateTime date, ApplicationUser resident, int addressId, bool isRegular);

        void EditMounthProperyIncome(int propertyId, int month, int year, decimal newPrice, bool isRegular);

        void DeleteIncome(int incomeId, int addressId);

        PagingServiceModel<AllIncomeServiceModel> GetAll(int addressId,  int page, string propertyId, DateTime startDate, DateTime endDate);
    }
}
