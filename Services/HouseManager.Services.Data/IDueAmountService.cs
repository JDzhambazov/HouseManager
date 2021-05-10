﻿namespace HouseManager.Services.Data
{
    using System.Collections.Generic;

    using HouseManager.Data.Models;
    using HouseManager.Web.ViewModels.DueAmount;

    public interface IDueAmountService
    {
        void AddMounthDueAmountInProperies(int propertyId, int month, int year);

        void AddMounthDueAmountInAllProperies(int addressId);

        void AddStartDueAmount(int propertyId, int month, int year, decimal cost, bool isRegular);

        void EditMountDueAmount(int month, int year, int propertyId, decimal cost, bool isRegular);

        (decimal RegularDueAmount, decimal NotRegularDueAmount) GetPropertyMountDueAmount(int propertyId);

        ICollection<MounthAmountViewModel> GetAddressDueAmount(ICollection<Property> properties);
    }
}
