namespace HouseManager.Web.ViewModels.Incomes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using HouseManager.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AddIncomeFormModel
    {
        public int PropertyId { get; set; }

        public string RegularIncome { get; set; }

        public DateTime RegularIncomeDate { get; set; }

        public string NotRegularIncome { get; set; }

        public DateTime NotRegularIncomeDate { get; set; }

        [Required]
        public string Resident { get; set; }

        public SelectList Residents { get; set; }
    }
}
