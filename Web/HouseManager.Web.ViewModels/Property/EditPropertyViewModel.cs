namespace HouseManager.Web.ViewModels.Property
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using HouseManager.Data.Models;

    public class EditPropertyViewModel
    {
        [Range(1,int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Range(1,int.MaxValue)]
        public int PropertyTypeId { get; set; }

        [Required]
        public PropertyType PropertyType { get; set; }

        [Range(0,20)]
        public int ResidentsCount { get; set; }

        [Range(1,int.MaxValue)]
        public int AddressId { get; set; }

        public Address Address { get; set; }

        public virtual ICollection<ApplicationUser> Residents { get; set; }

        //public virtual ICollection<RegularDueAmount> RegularDueAmounts { get; set; }

        //public virtual ICollection<NotRegularDueAmount> NotRegularDueAmounts { get; set; }

        //public virtual ICollection<RegularIncome> RegularIncomes { get; set; }

        //public virtual ICollection<NotRegularIncome> NotRegularIncomes { get; set; }

        //public ICollection<MonthFee> MonthFees { get; set; }
    }
}
