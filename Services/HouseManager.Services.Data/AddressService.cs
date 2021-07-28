namespace HouseManager.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HouseManager.Data.Common.Repositories;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data.Models;
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

        public AddressService(
            IDeletableEntityRepository<Address> addressRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IRepository<City> cityRepository,
            IRepository<District> districtRepository,
            IRepository<Property> propertyRepository,
            IRepository<Street> streetRepository,
            IRepository<MonthFee> monthFeeRepository)
        {
            this.addressRepository = addressRepository;
            this.userRepository = userRepository;
            this.cityRepository = cityRepository;
            this.districtRepository = districtRepository;
            this.propertyRepository = propertyRepository;
            this.streetRepository = streetRepository;
            this.monthFeeRepository = monthFeeRepository;
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

        public async Task SetAddressManager(int addressId, string userFullName)
        {
            var address = GetAddressById(addressId);
            var manager = GetUserByFullName(userFullName);
            address.Manager = manager;
            this.addressRepository.Update(address);
            await this.addressRepository.SaveChangesAsync();
        }

        public async Task SetAddressPaymaster(int addressId, string userFullName)
        {
            var address = GetAddressById(addressId);
            var payMaster = GetUserByFullName(userFullName);
            address.Paymaster = payMaster;
            await this.addressRepository.SaveChangesAsync();
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

        private ApplicationUser GetUserByFullName(string userFullName)
            => this.userRepository.All().FirstOrDefault(x => x.FullName == userFullName);
    }
}
