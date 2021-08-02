namespace HouseManager.Web.Areas.Statisics.Contollers
{
    using System.Xml;
    using HouseManager.Services.Data;
    using HouseManager.Web.Controllers;
    using HouseManager.Web.Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("Statistics")]
    [Authorize]
    public class IncomeController : BaseController
    {
        private readonly IIncomeService incomeService;
        private readonly IUserService userService;

        public IncomeController(IIncomeService incomeService,
            IUserService userService
            )
        {
            this.incomeService = incomeService;
            this.userService = userService;
        }

        public IIncomeService IncomeService { get; }

        public IActionResult GetAll([FromQuery] int currentPage)
        {
            ViewBag.isUserMakeChanges = this.userService
                .IsUserMakeChanges(this.User.Id(), this.GetAddressId());
            var incomes = this.incomeService.GetAll(this.GetAddressId(), currentPage);
            return View(incomes);
        }
    }
}
