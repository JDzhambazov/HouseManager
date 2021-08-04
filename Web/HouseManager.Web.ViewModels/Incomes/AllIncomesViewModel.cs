namespace HouseManager.Web.ViewModels.Incomes
{
    using HouseManager.Services.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AllIncomesViewModel
    {
        public IEnumerable<AllIncomeServiceModel> IncomeList { get; set; }

        public string PropertyId { get; set; }

        public IEnumerable<SelectListItem> Properties { get; set; }

        public DateTime? StartDate { get; set; } = null;

        public DateTime? EndDate { get; set; } = null;

        public int CurrentPage { get; set; }

        public int MaxPages { get; set; }
    }
}
