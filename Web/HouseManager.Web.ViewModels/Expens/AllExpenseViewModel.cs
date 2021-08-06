namespace HouseManager.Web.ViewModels.Expens
{
    using System;
    using System.Collections.Generic;
    using HouseManager.Services.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AllExpenseViewModel
    {
        public IEnumerable<AllExpenseServiseModel> ExpenseList { get; set; }

        public DateTime? StartDate { get; set; } = null;

        public DateTime? EndDate { get; set; } = null;

        public PagingModel Paging { get; set; }

        public string TotalPrice { get; set; }
    }
}
