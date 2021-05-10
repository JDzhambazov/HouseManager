namespace HouseManager.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class MonthFee
    {
        public MonthFee()
        {
            this.Properties = new HashSet<Property>();
            this.Addresses = new HashSet<Address>();
        }

        public int Id { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }

        public int FeeTypeId { get; set; }

        public FeeType FeeType { get; set; }

        [MaxLength(6)]
        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }

        public bool IsPersonal { get; set; }

        public bool IsRegular { get; set; }

        public virtual ICollection<Property> Properties { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
