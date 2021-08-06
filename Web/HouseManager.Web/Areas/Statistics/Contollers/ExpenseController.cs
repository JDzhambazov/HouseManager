namespace HouseManager.Web.Areas.Statistics.Contollers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HouseManager.Services.Data;
    using HouseManager.Web.Controllers;
    using HouseManager.Web.Infrastructure;
    using HouseManager.Web.ViewModels.Expens;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("Statistics")]
    [Authorize]
    public class ExpenseController : BaseController
    {
        private readonly IUserService userService;
        private readonly IExpensService expensService;
        private readonly IPropertyService propertyService;

        public ExpenseController(IUserService userService,
            IExpensService expensService,
            IPropertyService propertyService)
        {
            this.userService = userService;
            this.expensService = expensService;
            this.propertyService = propertyService;
        }

        public IActionResult GetAll(int currentPage, string propertyId,
                 DateTime startDate, DateTime endDate)
        {
            ViewBag.isUserMakeChanges = this.userService
                .IsUserMakeChanges(this.User.Id(), this.GetAddressId());
            var expense = this.expensService.GetAll(this.GetAddressId(), currentPage, startDate, endDate);
            var viewExpence = new AllExpenseViewModel()
            {
                ExpenseList = expense.ItemList,
                Paging = expense.Paging,
                TotalPrice = expense.ItemList.Sum(x => x.Price).ToString(),
            };

            return View(viewExpence);
        }
    }
}
