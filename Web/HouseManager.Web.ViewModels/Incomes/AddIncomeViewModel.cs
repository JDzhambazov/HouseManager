namespace HouseManager.Web.ViewModels.Incomes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using HouseManager.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AddIncomeViewModel
    {
        public int PropertyId { get; set; }

        [Required]
        public string RegularIncome { get; set; }

        public DateTime RegularIncomeDate { get; set; }

        [Required]
        public string NotRegularIncome { get; set; }

        public DateTime NotRegularIncomeDate { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Resident { get; set; }

        public ICollection<SelectListItem> Residents { get; set; }
    }
}
