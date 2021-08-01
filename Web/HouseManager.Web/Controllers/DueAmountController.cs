namespace HouseManager.Web.Controllers
{
    using HouseManager.Services.Data;
    using HouseManager.Services.Data.Models;
    using HouseManager.Web.Infrastructure;
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

        public IActionResult MonthAmount([FromQuery]int currentPage)
        {
            ViewBag.isUserMakeChanges = this.userService
                .IsUserMakeChanges(this.User.Id(), this.GetAddressId());
            return this.View(this.MonthAmountList(currentPage));
        }

        private MonthAmountServiseModel MonthAmountList(int page)
         => dueAmountService
                .GetAddressDueAmount(this.GetAddressId(),page);
    }
}
