namespace HouseManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HouseManager.Data;
    using HouseManager.Data.Common.Repositories;
    using HouseManager.Data.Models;
    using HouseManager.Services.Mapping;
    using HouseManager.Web.ViewModels.Addresses;
    using HouseManager.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<Address> addressRepository;
        private readonly IRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(
            IDeletableEntityRepository<Address> addressRepository,
            IRepository<Property> propertyRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this.addressRepository = addressRepository;
            this.propertyRepository = propertyRepository;
            this.userRepository = userRepository;
            this.userManager = userManager;
        }

        public async Task<IdentityResult> AddNewUser(string userName, string firstName, string lastName, string email, string password)
        {
            var fullname = string.Join(' ', firstName, lastName).Trim();
            var user = new ApplicationUser
            {
                UserName = userName,
                FullName = fullname,
                Email = email,
            };

            var isEmailExist = userRepository
                .All()
                .Any(x => x.Email == email);

            if (isEmailExist)
            {
                var error = new IdentityError
                {
                    Code = "DuplicateEmailAddress",
                    Description = $"Има регистриран потребител с Email {email}",
                };

                return IdentityResult.Failed(error);
            }

            return await userManager.CreateAsync(user, password);

        }

        public IEnumerable<UserListViewModel> GetAllUsersInAddress(Address address)
        {
            var properties = propertyRepository
                .All()
                .Where(x => x.Address == address)
                .ToList();

            return GetUsers(properties);
        }

        public IEnumerable<UserListViewModel> GetAllUsersInAddress(int addressId)
        {
            var properties = propertyRepository
                .All()
                .Where(x => x.AddressId == addressId)
                .ToList();

            return GetUsers(properties);
        }


        public async Task<IEnumerable<AddressViewModel>> GetUserAddresses(string userName)
        {
            var user = this.userRepository.All().FirstOrDefault(x => x.UserName == userName);
            var result = new List<AddressViewModel>();

            var properties =
                this.propertyRepository.All()
                .Where(x => x.Residents.Contains(user))
                .Select(x => x.Address)
                .To<AddressViewModel>()
                .ToList();
                
                var addressList = this.addressRepository
                    .All()
                    .Where(x => x.Manager == user || x.Paymaster == user || x.Creator == user)
                    .To<AddressViewModel>()
                    .ToList();

            result.AddRange(addressList);
            result.AddRange(properties);

            foreach (var address in result)
            {
                ReplaseNames(address);
            }

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

        private static void ReplaseNames(AddressViewModel address)
        {
            if (address.CityName != null)
            {
                address.CityName = string.Concat("гр. " + address.CityName);
            }

            if (address.StreetName != null)
            {
                address.StreetName = string.Concat("ул. " + address.StreetName);
            }

            if (address.DistrictName != null)
            {
                address.DistrictName = string.Concat("ж.к. " + address.DistrictName);
            }

            if (address.Entrance != null)
            {
                address.Entrance = string.Concat("вх. " + address.Entrance);
            }

            if (address.Number != null)
            {
                if (address.DistrictName != null)
                {
                    address.Number = string.Concat("бл.№ " + address.Number);
                }
                else
                {
                    address.Number = string.Concat("№ " + address.Number);
                }
            }
        }
    }
}
