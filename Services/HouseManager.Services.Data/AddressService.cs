namespace HouseManager.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HouseManager.Data;
    using HouseManager.Data.Common.Repositories;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data;
    using HouseManager.Web.ViewModels.Addresses;

    public class AddressService : IAddressService
    {
        private readonly IDeletableEntityRepository<Address> addressRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IRepository<City> cityRepository;
        private readonly IRepository<District> districtRepository;
        private readonly IRepository<Property> propertyRepository;
        private readonly IRepository<Street> streetRepository;

        public AddressService(
            IDeletableEntityRepository<Address> addressRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IRepository<City> cityRepository,
            IRepository<District> districtRepository,
            IRepository<Property> propertyRepository,
            IRepository<Street> streetRepository)
        {
            this.addressRepository = addressRepository;
            this.userRepository = userRepository;
            this.cityRepository = cityRepository;
            this.districtRepository = districtRepository;
            this.propertyRepository = propertyRepository;
            this.streetRepository = streetRepository;
        }

        public async Task CreateAddress(string cityName, string districtName,
            string streetName, string number, string entrance,
            int numberOfProperties, ApplicationUser creatоr = null)
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
            };

            if (creatоr != null)
            {
                address.Manager = creatоr;
            }

            await this.addressRepository.AddAsync(address);
            await this.addressRepository.SaveChangesAsync();
        }

        public ICollection<Property> GetAllProperyies(int addressId)
        {
            return this.propertyRepository.All().Where(x => x.AddressId == addressId).ToList();
        }

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

        public async Task Delete(Address address)
        {
            this.addressRepository.Delete(address);
        }

        private Address GetAddressById(int addressId)
        {
            return this.addressRepository.All().FirstOrDefault(x => x.Id == addressId);
        }

        private ApplicationUser GetUserByFullName(string userFullName)
        {
            return this.userRepository.All().FirstOrDefault(x => x.FullName == userFullName);
        }
    }
}
