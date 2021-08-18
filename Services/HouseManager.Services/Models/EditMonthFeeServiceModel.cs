namespace HouseManager.Services.Models
{
    using HouseManager.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class EditMonthFeeServiceModel
    {
        public IEnumerable<MonthFee> AddressFees { get; set; }

        public DateTime StartDate { get; set; }

        [RegularExpression("^(\\d{0,4}.{0,1},{0,1}\\d{0,3})$")]
        public string Cost { get; set; }

        public int FeeId { get; set; }
    }
}
