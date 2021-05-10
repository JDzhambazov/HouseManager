namespace HouseManager.Services.Data
{
    using System.Collections.Generic;

    using HouseManager.Data.Models;

    public interface IFeeService
    {
        void AddFeeToAddress(int addressId, string feeName, decimal cost, bool isPersonel, bool isRegular);

        void AddFeeToProperty(ICollection<int> propertiesId, string feeName);

        void EditAddresFee(int addressId, string feeName, decimal cost);

        ICollection<MonthFee> GetAllFeesInAddress(int addressId);

        ICollection<MonthFee> GetAllFeesInProperty(int propertyId);
    }
}
