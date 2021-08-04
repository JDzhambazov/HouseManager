namespace HouseManager.Web.Areas.Statisics.Contollers
{
    using System;
    using System.Xml;
    using HouseManager.Services.Data;
    using HouseManager.Web.Controllers;
    using HouseManager.Web.Infrastructure;
    using HouseManager.Web.ViewModels.Incomes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("Statistics")]
    [Authorize]
    public class IncomeController : BaseController
    {
        private readonly IIncomeService incomeService;
        private readonly IUserService userService;
        private readonly IPropertyService propertyService;

        public IncomeController(IIncomeService incomeService,
            IUserService userService,
            IPropertyService propertyService
            )
        {
            this.incomeService = incomeService;
            this.userService = userService;
            this.propertyService = propertyService;
        }

        public IIncomeService IncomeService { get; }

        public IActionResult GetAll(int currentPage, string propertyId, 
            DateTime startDate, DateTime endDate)
        {
            ViewBag.isUserMakeChanges = this.userService
                .IsUserMakeChanges(this.User.Id(), this.GetAddressId());
            var incomes = this.incomeService.GetAll(this.GetAddressId(), currentPage,propertyId,startDate,endDate);
            var viewIncomes = new AllIncomesViewModel()
            {
                IncomeList = incomes.ItemList,
                Paging = incomes.Paging,
                Properties = this.AddNullValueInSelectListItem(propertyService.GetPropertiesInAddress(this.GetAddressId())),
            };

            return View(viewIncomes);
        }

        public IActionResult Edit(int incomeId)
        {
            return View();
        }

        public IActionResult Delete(int incomeId)
        {
            return RedirectToAction(nameof(GetAll));
        }
    }
}
