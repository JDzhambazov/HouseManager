namespace HouseManager.Services.Data
{
    using HouseManager.Data.Models;
    using HouseManager.Web.ViewModels.Users;
    using System;
    using System.Collections.Generic;

    public interface IUserService
    {
        void AddNewUser(string userName, string firstName, string lastName, string email, string password);

        List<UserListViewModel> GetAllUsersInAddress(Address address);

        List<UserListViewModel> GetAllUsersInAddress(int addressId);
    }
}
