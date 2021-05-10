namespace HouseManager.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Property
    {
        public Property()
        {
            this.Residents = new HashSet<ApplicationUser>();
            this.RegularIncomes = new HashSet<RegularIncome>();
            this.NotRegularIncomes = new HashSet<NotRegularIncome>();
            this.RegularDueAmounts = new HashSet<RegularDueAmount>();
            this.NotRegularDueAmounts = new HashSet<NotRegularDueAmount>();
            this.MonthFees = new HashSet<MonthFee>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public int PropertyTypeId { get; set; }

        public PropertyType PropertyType { get; set; }

        [MaxLength(2)]
        public int ResidentsCount { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }

        public virtual ICollection<ApplicationUser> Residents { get; set; }

        public virtual ICollection<RegularDueAmount> RegularDueAmounts { get; set; }

        public virtual ICollection<NotRegularDueAmount> NotRegularDueAmounts { get; set; }

        public virtual ICollection<RegularIncome> RegularIncomes { get; set; }

        public virtual ICollection<NotRegularIncome> NotRegularIncomes { get; set; }

        public ICollection<MonthFee> MonthFees { get; set; }
    }
}
