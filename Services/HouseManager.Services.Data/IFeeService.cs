namespace HouseManager.Services.Data
{
    using System.Collections.Generic;

    using HouseManager.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IFeeService
    {
        void AddFeeToAddress(int addressId, string feeName, decimal cost, bool isPersonel, bool isRegular);

        void AddFeeToProperty(int propertyId, int feeId);

        void EditFee(int feeId, decimal cost);

        SelectList GetAllFees();

        ICollection<MonthFee> GetAllFeesInAddress(int addressId);

        ICollection<MonthFee> GetAllFeesInProperty(int propertyId);
    }
}
