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
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IEnumerable<SelectListItem> GetAllUsersInAddress(int addressId)
         => propertyRepository
                .All()
                .Where(x => x.AddressId == addressId)
                .SelectMany(x => x.Residents
                .Select(n => new SelectListItem { Value =n.Id ,Text = n.FullName}))
                .ToList();

        public AddressViewModel CurrentAddressInfo(int addressId)
        {
            var currentAddres = addressRepository.All()
                .Where(x => x.Id == addressId)
                .To<AddressViewModel>()
                .FirstOrDefault();
            
            ReplaseNames(currentAddres);

            return currentAddres ;
        }

        public IEnumerable<AddressViewModel> GetUserAddresses(string userName)
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


        public ApplicationUser GetUserById(string userId)
            => this.userRepository.All().FirstOrDefault(x => x.Id == userId);

        public bool IsUserMakeChanges(string userId, int addressId)
        {
            if(addressId <= 0)
            {
                return false;
            }

            return this.addressRepository
                    .All()
                    .Where(x => x.Id == addressId)
                    .Any(x => x.ManagerId == userId || x.PaymasterId == userId || x.CreatorId == userId);
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
