namespace HouseManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HouseManager.Services.Data;
    using HouseManager.Services.Models;
    using HouseManager.Web.ViewModels.DueAmount;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class DueAmountController : BaseController
    {
        private readonly IDueAmountService dueAmountService;
        private readonly IAddressService addressService;

        public DueAmountController(IDueAmountService dueAmountService, IAddressService addressService)
        {
            this.dueAmountService = dueAmountService;
            this.addressService = addressService;
        }

        public IActionResult MonthAmount() 
            => this.View(this.MonthAmountList());

        private MonthAmountViewModel MonthAmountList(int page = 1)
        {
            var result = new MonthAmountViewModel();
            result.MonthAmounts = dueAmountService
                .GetAddressDueAmount(this.GetAddressId(),page);
            if (result.MonthAmounts.Count() > 10)
            {
                result.Pages = new PagingServiceModel { MaxPages = 2 };
            }
            return result;
        }
    }
}
