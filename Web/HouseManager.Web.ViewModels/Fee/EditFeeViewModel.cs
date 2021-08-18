namespace HouseManager.Web.ViewModels.Fee
{
    using HouseManager.Data.Models;
    using HouseManager.Services.Models;
    using System;
    using System.Collections.Generic;

    public class EditFeeViewModel
    {
        public IEnumerable<MonthFee> AddressFees { get; set; }

        public DateTime StartDate { get; set; }

        public EditMonthFeeServiceModel FeeDetails { get; set; }
    }
}
