namespace HouseManager.Web.Areas.Statisics.Contollers
{
    using System;
    using System.Xml;
    using HouseManager.Services.Data;
    using HouseManager.Services.Models;
    using HouseManager.Web.Controllers;
    using HouseManager.Web.Infrastructure;
    using HouseManager.Web.ViewModels.Incomes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Edit(int id)
        {
            var income = incomeService.GetById(id, this.GetAddressId());


            if (income == null)
            {
                return BadRequest();
            }

            var viewIncome = new EditIncomeViewModel
            {
                Id = income.Id,
                PropertyId = income.PropertyId,
                PropertyName = income.PropertyName,
                Price = income.Price.ToString(),
                Date = income.Date,
                IncomeName = income.IncomeName,
                ResidentId = income.ResidentId,
                Residents = propertyService.GetAllResidents((int)income.PropertyId),
            };

            return View(viewIncome);
        }

        [HttpPost]
        public IActionResult Edit(EditIncomeViewModel income)
        {
            var price = this.DecimalValue(income.Price);

            if(price <= 0)
            {
                ModelState.AddModelError(string.Empty, "Платената сума трябва да е положително число");
            }

            if(income.Date > DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Датата на плащане трябва да е преди текущата");
            }

            if (!ModelState.IsValid)
            {
                income.Residents = propertyService.GetAllResidents((int)income.PropertyId);
                return View(income);
            }

            this.incomeService.EditIncome(income.Id,
                price, income.Date, income.ResidentId, this.GetAddressId());

            return RedirectToAction(nameof(GetAll));
        }

        public IActionResult Delete(int id)
        {
            this.incomeService.DeleteIncome(id, this.GetAddressId());
            return RedirectToAction(nameof(GetAll));
        }
    }
}
