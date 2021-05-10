namespace HouseManager.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using HouseManager.Data;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data;

    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext db;

        public AddressService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void CreateAddress(string cityName, string districtName, string streetName, string number, string entrance, int numberOfProperties)
        {
            District district = null;
            Street street = null;
            var city = this.db.Cities.FirstOrDefault(x => x.Name == cityName);
            if (districtName != null)
            {
                district = this.db.Districts.FirstOrDefault(x => x.Name == districtName)
                    ?? new District { Name = districtName };
            }

            if (streetName != null)
            {
                street = this.db.Streets.FirstOrDefault(x => x.Name == streetName)
                    ?? new Street { Name = streetName };
            }

            this.db.Addresses.Add(new Address
            {
                City = city ?? new City { Name = cityName },
                District = district,
                Street = street,
                Number = number,
                Entrance = entrance,
                NumberOfProperties = numberOfProperties,
            });
            this.db.SaveChanges();
        }

        public ICollection<Property> GetAllProperyies(int addressId)
        {
            return this.db.Properties.Where(x => x.AddressId == addressId).ToList();
        }

        public void SetAddressManager(int addressId, string userFullName)
        {
            var address = this.db.Addresses.FirstOrDefault(x => x.Id == addressId);
            var manager = this.db.Users.FirstOrDefault(x => x.FullName == userFullName);
            address.Manager = manager;
            this.db.SaveChanges();
        }

        public void SetAddressPaymaster(int addressId, string userFullName)
        {
            var address = this.db.Addresses.FirstOrDefault(x => x.Id == addressId);
            var payMaster = this.db.Users.FirstOrDefault(x => x.FullName == userFullName);
            address.Paymaster = payMaster;
            this.db.SaveChanges();
        }
    }
}
