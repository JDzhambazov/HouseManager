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

        void EditIncome(int incomeId, decimal newPrice,DateTime date,string residentId , int addressId);

        void DeleteIncome(int incomeId, int addressId);

        string IncomeConfirmationМessage(string regularIncome, string notRegularIncome, string payer);

        PagingServiceModel<AllIncomeServiceModel> GetAll(int addressId,  int page, string propertyId, DateTime startDate, DateTime endDate);

        EditIncomeServiceModel GetById(int incomeId, int addressId);
    }
}
