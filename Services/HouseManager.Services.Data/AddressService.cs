namespace HouseManager.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HouseManager.Data;
    using HouseManager.Data.Common.Repositories;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AddressService : IAddressService
    {
        private readonly IDeletableEntityRepository<Address> addressRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IRepository<City> cityRepository;
        private readonly IRepository<District> districtRepository;
        private readonly IRepository<Property> propertyRepository;
        private readonly IRepository<Street> streetRepository;
        private readonly IRepository<MonthFee> monthFeeRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly string managerRole = "Manager";
        private readonly string payMasterRole = "PayMaster";

        public AddressService(
            IDeletableEntityRepository<Address> addressRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IRepository<City> cityRepository,
            IRepository<District> districtRepository,
            IRepository<Property> propertyRepository,
            IRepository<Street> streetRepository,
            IRepository<MonthFee> monthFeeRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.addressRepository = addressRepository;
            this.userRepository = userRepository;
            this.cityRepository = cityRepository;
            this.districtRepository = districtRepository;
            this.propertyRepository = propertyRepository;
            this.streetRepository = streetRepository;
            this.monthFeeRepository = monthFeeRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<int> CreateAddress(string cityName, string districtName,
            string streetName, string number, string entrance,
            int numberOfProperties, string creatоrId)
        {
            District district = null;
            Street street = null;
            var city = this.cityRepository.All().FirstOrDefault(x => x.Name == cityName)
                ?? new City { Name = cityName };
            if (districtName != null)
            {
                district = this.districtRepository.All().FirstOrDefault(x => x.Name == districtName)
                    ?? new District { Name = districtName };
            }

            if (streetName != null)
            {
                street = this.streetRepository.All().FirstOrDefault(x => x.Name == streetName)
                    ?? new Street { Name = streetName };
            }

            var address = new Address
            {
                City = city ,
                District = district,
                Street = street,
                Number = number,
                Entrance = entrance,
                NumberOfProperties = numberOfProperties,
                CreatorId = creatоrId,
            };
           
            await this.addressRepository.AddAsync(address);
            await this.addressRepository.SaveChangesAsync();
            return address.Id;
        }

        public AdressServiseInputModel GetSelectItems(AdressServiseInputModel adrress)
        {
            var allCities = cityRepository.All()
                .Select(x => new SelectListItem
                {
                    Value = x.Name,
                    Text = x.Name,
                })
                .ToList();
            allCities.Add(new SelectListItem
            {
                Value = "",
                Text = "",
                Selected = true,
            });

            var allDistricts = districtRepository.All()
                .Select(x => new SelectListItem
                {
                    Value = x.Name,
                    Text = x.Name,
                })
                .ToList();
            allDistricts.Add(new SelectListItem
            {
                Value = "",
                Text = "",
                Selected = true,
            });

            var allStreets = streetRepository.All()
                .Select(x => new SelectListItem
                {
                    Value = x.Name,
                    Text = x.Name,
                })
                .ToList();
            allStreets.Add(new SelectListItem
            {
                Value = "",
                Text = "",
                Selected = true,
            });

            adrress.AllCities = allCities;
            adrress.AllDistricts = allDistricts;
            adrress.AllStreets = allStreets;

            return adrress;
        }

        public ICollection<SelectListItem> GetAddressMounthFees(int addressId)
            => this.monthFeeRepository.All().Where(x => x.AddressId == addressId)
            .Select(x => new SelectListItem
            {
                Value = x.FeeType.Name ,
                Text = x.FeeType.Name,
            }).ToList();

        public ICollection<Property> GetAllProperyies(int addressId) 
            => this.propertyRepository.All().Where(x => x.AddressId == addressId).ToList();

        public int GetPropertyCount(int addressId)
            => this.addressRepository.All().Where(x => x.Id == addressId)
            .Select(x => x.NumberOfProperties - x.Properties.Count).FirstOrDefault();

        public async Task SetAddressManager(int addressId, string userId)
        {
            var address = GetAddressById(addressId);
            var roleExist = await this.roleManager.RoleExistsAsync(managerRole);

            if (!roleExist)
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = managerRole });
            }

            if (address.ManagerId != null)
            {
                var manager = userRepository.All().FirstOrDefault(x => x.Id == address.ManagerId);
                await this.userManager.RemoveFromRoleAsync(manager, managerRole);
            }

            var user = userRepository.All().FirstOrDefault(x => x.Id == userId);
            var addToRole = await this.userManager.AddToRoleAsync(user, managerRole);

            if (addToRole.Succeeded)
            {
                address.ManagerId = userId;
                this.addressRepository.Update(address);
                await this.addressRepository.SaveChangesAsync();
            }
        }

        public async Task SetAddressPaymaster(int addressId, string userId)
        {
            var address = GetAddressById(addressId);
            var roleExist = await this.roleManager.RoleExistsAsync(payMasterRole);

            if (!roleExist)
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = payMasterRole });
            }

            if (address.PaymasterId != null)
            {
                var payMaster = userRepository.All().FirstOrDefault(x => x.Id == address.PaymasterId);
                await this.userManager.RemoveFromRoleAsync(payMaster, payMasterRole);
            }

            var user = userRepository.All().FirstOrDefault(x => x.Id == userId);
            var addToRole = await this.userManager.AddToRoleAsync(user, payMasterRole);

            if (addToRole.Succeeded)
            {
                address.PaymasterId = userId;
                this.addressRepository.Update(address);
                await this.addressRepository.SaveChangesAsync();
            }
        }

        public async Task<bool> SetCurrentAddressId(int currentAddressId, ApplicationUser user)
        {

            var address = addressRepository.All()
                .Where(x => x.Manager == user || x.Paymaster == user
                || x.Properties.Any(p => p.Residents.Contains(user)))
                .ToList();

            if (address == null)
            {
                return false;
            }

            user.CurrentAddressId = currentAddressId;
            userRepository.Update(user);
            await userRepository.SaveChangesAsync();
            return true;
        }

        public void Delete(Address address)
            => this.addressRepository.Delete(address);

        private Address GetAddressById(int addressId)
            => this.addressRepository.All().FirstOrDefault(x => x.Id == addressId);
    }
}
