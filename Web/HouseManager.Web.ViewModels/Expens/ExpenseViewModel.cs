using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager.Web.ViewModels.Expens
{
    public class ExpenseViewModel
    {
        public int AddressId { get; set; }

        public SelectList ExpensTypeList { get; set; }

        public string ExpensType { get; set; }

        public string Price { get; set; }

        public DateTime Date { get; set; }

        public bool IsRegular { get; set; }
    }
}
