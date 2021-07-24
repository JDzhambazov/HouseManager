namespace HouseManager.Services.Data
{
    using HouseManager.Data.Models;
    using HouseManager.Web.ViewModels.Addresses;
    using HouseManager.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<IdentityResult> AddNewUser(string userName, string firstName, string lastName, string email, string password);

        IEnumerable<UserListViewModel> GetAllUsersInAddress(Address address);

        IEnumerable<UserListViewModel> GetAllUsersInAddress(int addressId);

        Task<IEnumerable<AddressViewModel>> GetUserAddresses(string user);
    }
}
