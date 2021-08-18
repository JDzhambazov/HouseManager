namespace HouseManager.Services.Models
{
    using HouseManager.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class EditMonthFeeServiceModel
    {
        public IEnumerable<MonthFee> AddressFees { get; set; }

        public DateTime StartDate { get; set; }

        public string Cost { get; set; }

        public int FeeId { get; set; }
    }
}
