namespace HouseManager.Web.ViewModels.DueAmount
{
    using HouseManager.Services.Models;
    using System.Collections.Generic;

    public class MonthAmountViewModel
    {
        public IEnumerable<MonthAmountServiseModel> MonthAmounts { get; set; }

        public PagingServiceModel Pages { get; set; } = null;
    }
}
