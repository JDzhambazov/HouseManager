namespace HouseManager.Web.Controllers
{
    using System.Linq;
    using HouseManager.Services.Data;
    using HouseManager.Services.Models;
    using HouseManager.Web.Infrastructure;
    using HouseManager.Web.ViewModels.DueAmount;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class DueAmountController : BaseController
    {
        private readonly IDueAmountService dueAmountService;
        private readonly IAddressService addressService;
        private readonly IUserService userService;

        public DueAmountController(IDueAmountService dueAmountService,
            IAddressService addressService,
            IUserService userService)
        {
            this.dueAmountService = dueAmountService;
            this.addressService = addressService;
            this.userService = userService;
        }

        public IActionResult MonthAmount()
        {
            ViewBag.isUserMakeChanges = this.userService
                .IsUserMakeChanges(this.User.Id(), this.GetAddressId());
            return this.View(this.MonthAmountList());
        }


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
