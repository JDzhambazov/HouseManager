namespace HouseManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HouseManager.Data;
    using HouseManager.Data.Common.Repositories;
    using HouseManager.Data.Models;
    using HouseManager.Web.ViewModels.Addresses;
    using HouseManager.Web.ViewModels.Users;
    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<Address> addressRepository;
        private readonly IRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public UserService(
            IDeletableEntityRepository<Address> addressRepository,
            IRepository<Property> propertyRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository
            )
        {
            this.addressRepository = addressRepository;
            this.propertyRepository = propertyRepository;
            this.userRepository = userRepository;
        }

        public async Task AddNewUser(string userName, string firstName, string lastName, string email, string password)
        {
            var fullname = string.Join(' ', firstName, lastName).Trim();

            await this.userRepository.AddAsync(new ApplicationUser
            {
                UserName = userName,
                FullName = fullname,
                NormalizedUserName = userName.ToUpper(),
                Email = email != null ? email : null,
                NormalizedEmail = email != null ? email.ToUpper() : null,
                CreatedOn = DateTime.UtcNow,
                PasswordHash = password,
                EmailConfirmed = true,
            });
            await this.userRepository.SaveChangesAsync();
        }

        public IEnumerable<UserListViewModel> GetAllUsersInAddress(Address address)
        {
            var properties = propertyRepository.All().Where(x => x.Address == address).ToList();
            return GetUsers(properties);
        }

        public IEnumerable<UserListViewModel> GetAllUsersInAddress(int addressId)
        {
            var properties = propertyRepository.All().Where(x => x.AddressId == addressId).ToList();
            return GetUsers(properties);
        }


        public async Task<IEnumerable<AddressViewModel>> GetUserAddresses(string userName)
        {
            var user = this.userRepository.All().FirstOrDefault(x => x.UserName == userName);
            var result = new List<AddressViewModel>();

            var properties =
                this.propertyRepository.All()
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
                .ToList<AddressViewModel>();
                
                var addressList = this.addressRepository
                    .All()
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
                    .ToList<AddressViewModel>();
            result.AddRange(addressList);
            result.AddRange(properties);

            return result = result.Distinct().ToList();
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
