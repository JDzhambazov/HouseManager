namespace HouseManager.Services.Data
{
    using HouseManager.Data.Models;
    using HouseManager.Web.ViewModels.Addresses;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<IdentityResult> AddNewUser(string userName, string firstName, string lastName, string email, string password);

        AddressViewModel CurrentAddressInfo(int addressId);

        IEnumerable<SelectListItem> GetAllUsersInAddress(int addressId);

        IEnumerable<AddressViewModel> GetUserAddresses(string user);

        ApplicationUser GetUserById(string userId);

        bool IsUserMakeChanges(string userId , int addressId);
    }
}
