namespace HouseManager.Services.Data
{
    using System;
    using System.Collections.Generic;

    using HouseManager.Data.Models;
    using HouseManager.Services.Data.Models;
    using HouseManager.Services.Models;

    public interface IDueAmountService
    {
        void AddMounthDueAmountInProperies(int propertyId, int month, int year);

        void AddMounthDueAmountInAllProperies(int addressId);

        void AddStartDueAmount(int propertyId, int month, int year, decimal cost, bool isRegular);

        void EditPropertyMountDueAmount(int propertyId, DateTime startDate);

        void EditMountDueAmount(int month, int year, int propertyId, decimal cost, bool isRegular);

        (decimal RegularDueAmount, decimal NotRegularDueAmount) GetPropertyMountDueAmount(int propertyId);

        PagingServiceModel<AmountListServiceModel> GetAddressDueAmount(int addressId, int page = 1);
    }
}
