namespace HouseManager.Data.Models
{
    using HouseManager.Data.Common.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Address : BaseDeletableModel<int>
    {
        public Address()
        {
            this.Properties = new HashSet<Property>();
            this.Expenses = new HashSet<Expens>();
            this.RegularIncomes = new HashSet<RegularIncome>();
            this.NotRegularIncomes = new HashSet<NotRegularIncome>();
            this.MonthlyFees = new HashSet<MonthFee>();
        }

        public int CityId { get; set; }

        public City City { get; set; }

        public int? DistrictId { get; set; }

        public District District { get; set; }

        public int? StreetId { get; set; }

        public Street Street { get; set; }

        [Required]
        [MaxLength(5)]
        public string Number { get; set; }

        [MaxLength(5)]
        public string Entrance { get; set; }

        [MaxLength(2)]
        public int NumberOfProperties { get; set; }
        
        public string CreatorId { get; set; }

        [InverseProperty(nameof(ApplicationUser.Creators))]
        public virtual ApplicationUser Creator { get; set; }

        public string ManagerId { get; set; }

        [InverseProperty(nameof(ApplicationUser.Managers))]
        public virtual ApplicationUser Manager { get; set; }

        public string PaymasterId { get; set; }

        [InverseProperty(nameof(ApplicationUser.Paymasters))]
        public virtual ApplicationUser Paymaster { get; set; }

        public virtual ICollection<Property> Properties { get; set; }

        public virtual ICollection<RegularIncome> RegularIncomes { get; set; }

        public virtual ICollection<NotRegularIncome> NotRegularIncomes { get; set; }

        public virtual ICollection<Expens> Expenses { get; set; }

        public virtual ICollection<MonthFee> MonthlyFees { get; set; }
    }
}
