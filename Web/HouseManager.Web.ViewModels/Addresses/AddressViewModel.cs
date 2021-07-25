namespace HouseManager.Web.ViewModels.Addresses
{
    using System;
    using HouseManager.Data.Models;
    using HouseManager.Services.Mapping;

    public class AddressViewModel :IEquatable<AddressViewModel>, IMapFrom<Address>, IMapFrom<Property>
    {
        public int AddressId { get; set; }

        public string CityName { get; set; }

        public string DistrictName { get; set; }

        public string StreetName { get; set; }

        public string Number { get; set; }

        public string Entrance { get; set; }

        public bool Equals(AddressViewModel other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (Object.ReferenceEquals(this, other)) return true;

            return AddressId.Equals(other.AddressId);
        }

        public override int GetHashCode()
        {
            int hashAddressId = AddressId.GetHashCode();

            return hashAddressId;
        }
    }
}
