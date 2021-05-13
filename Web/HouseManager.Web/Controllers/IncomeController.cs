namespace HouseManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HouseManager.Services.Data;
    using HouseManager.Web.ViewModels.DueAmount;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class IncomeController : Controller
    {
        private readonly IDueAmountService dueAmountService;

        public IncomeController(IDueAmountService dueAmountService)
        {
            this.dueAmountService = dueAmountService;
        }

        public ActionResult AddIncome(int id) 
        {
            var currentAmount = dueAmountService.GetPropertyMountDueAmount(id);
            var result = new MounthAmountViewModel
            {
                Id = id,
                NotRegularDueAmount = currentAmount.NotRegularDueAmount,
                RegularDueAmount = currentAmount.RegularDueAmount,
            };
            return View(result);
        }
    }
}
