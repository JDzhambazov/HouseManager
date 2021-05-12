namespace HouseManager.Services.Data
{
    using HouseManager.Data.Models;
    using HouseManager.Web.ViewModels.Addresses;
    using HouseManager.Web.ViewModels.Users;
    using System;
    using System.Collections.Generic;

    public interface IUserService
    {
        void AddNewUser(string userName, string firstName, string lastName, string email, string password);

        IEnumerable<UserListViewModel> GetAllUsersInAddress(Address address);

        IEnumerable<UserListViewModel> GetAllUsersInAddress(int addressId);

        IEnumerable<AddressViewModel> GetUserAddresses(string user);
    }
}
