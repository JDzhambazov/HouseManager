namespace HouseManager.Services.Data
{
    using System.Collections.Generic;

    using HouseManager.Data.Models;

    public interface IAddressService
    {
        void CreateAddress(string cityName, string districtName, string streetName, string number, string entrance, int numberOfProperties);

        void SetAddressManager(int addressId, string userFullName);

        void SetAddressPaymaster(int addressId, string userFullName);

        ICollection<Property> GetAllProperyies(int addressId);
    }
}
