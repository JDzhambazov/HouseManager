namespace HouseManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using HouseManager.Data;
    using HouseManager.Data.Models;
    using HouseManager.Web.ViewModels.Users;

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext db;

        public UserService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddNewUser(string userName, string firstName, string lastName, string email, string password)
        {
            var fullname = string.Join(' ', firstName, lastName).Trim();

            this.db.Users.Add(new ApplicationUser
            {
                UserName = userName,
                FullName = fullname,
                NormalizedUserName = userName.ToUpper(),
                Email = email != null ? email : null,
                NormalizedEmail = email != null ? email.ToUpper() : null,
                CreatedOn = DateTime.Now,
                PasswordHash = password,
                EmailConfirmed = true,
            });
            this.db.SaveChanges();
        }

        public List<UserListViewModel> GetAllUsersInAddress(Address address)
        {
            var properties = db.Properties.Where(x => x.Address == address).ToList();
            return GetUsers(properties);
        }

        public List<UserListViewModel> GetAllUsersInAddress(int addressId)
        {
            var properties = db.Properties.Where(x => x.AddressId == addressId).ToList();
            return GetUsers(properties);
        }

        private static List<UserListViewModel> GetUsers(List<Property> properties)
        {
            var userList = new List<UserListViewModel>();
            foreach (var property in properties)
            {
                foreach (var user in property.Residents)
                {
                    userList.Add(new UserListViewModel
                    {
                        FullName = user.FullName,
                    });
                }
            }

            return userList;
        }
    }
}
