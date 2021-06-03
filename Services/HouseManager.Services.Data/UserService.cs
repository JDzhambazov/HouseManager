namespace HouseManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using HouseManager.Data;
    using HouseManager.Data.Models;
    using HouseManager.Web.ViewModels.Addresses;
    using HouseManager.Web.ViewModels.Users;
    using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<UserListViewModel> GetAllUsersInAddress(Address address)
        {
            var properties = db.Properties.Where(x => x.Address == address).ToList();
            return GetUsers(properties);
        }

        public IEnumerable<UserListViewModel> GetAllUsersInAddress(int addressId)
        {
            // return this.db.Users.Select(x => new UserListViewModel { FullName = x.FullName }).ToList();
            var properties = db.Properties.Where(x => x.AddressId == addressId).ToList();
            return GetUsers(properties);
        }


        public IEnumerable<AddressViewModel> GetUserAddresses(string userName)
        {
            var user = this.db.Users.FirstOrDefault(x => x.UserName == userName);

            var addresses =
                this.db.Properties
                .Where(x => x.Residents.Contains(user))
                .Select(x => new AddressViewModel
                {
                    AddressId = x.AddressId,
                    CityName = x.Address.City.Name,
                    DistrictName = x.Address.District.Name,
                    StreetName = x.Address.Street.Name,
                    Number = x.Address.Number,
                    Entrance = x.Address.Entrance,
                })
                .ToList();
                
                var addressList = addresses.Count > 0 ? addresses : this.db.Addresses
                    .Where(x => x.Manager == user)
                    .Select(x => new AddressViewModel
                    {
                        AddressId = x.Id,
                        CityName = x.City.Name,
                        DistrictName = x.District.Name,
                        StreetName = x.Street.Name,
                        Number = x.Number,
                        Entrance = x.Entrance,
                    })
                    .ToList();

            return addressList;
         }

        private static IEnumerable<UserListViewModel> GetUsers(IEnumerable<Property> properties)
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
