namespace HouseManager.Web.ViewModels.Incomes
{
    using HouseManager.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;

    public class AddIncomeViewModel
    {
        public int PropertyId { get; set; }

        public string RegularIncome { get; set; }

        public DateTime RegularIncomeDate { get; set; }

        public string NotRegularIncome { get; set; }

        public DateTime NotRegularIncomeDate { get; set; }

        public string Resident { get; set; }

        public ICollection<SelectListItem> Residents { get; set; }
    }
}
