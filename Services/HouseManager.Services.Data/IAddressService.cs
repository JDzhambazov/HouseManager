namespace HouseManager.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HouseManager.Data.Models;
    using HouseManager.Services.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IAddressService
    {
       
        Task<int> CreateAddress(string cityName, string districtName, string streetName, string number, string entrance, int numberOfProperties, string creatоrId);

        Task SetAddressManager(int addressId, string userFullName);

        Task SetAddressPaymaster(int addressId, string userFullName);

        ICollection<SelectListItem> GetAddressMounthFees (int addressId);

        ICollection<Property> GetAllProperyies(int addressId);

        int GetPropertyCount(int addressId);

        AdressServiseInputModel GetSelectItems(AdressServiseInputModel adrress);

        void Delete(Address address);

        Task<bool> SetCurrentAddressId(int id , ApplicationUser user);
    }
}
