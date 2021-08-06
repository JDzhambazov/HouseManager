namespace HouseManager.Web.Areas.Statistics.Contollers
{
    using System;
    using System.Linq;

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

        public IActionResult Edit(int id)
        {
            var expense = expensService.GetById(id, this.GetAddressId());


            if (expense == null)
            {
                return BadRequest();
            }

            var expenseTypes = expensService.GetExpenseTypes(this.GetAddressId());

            var viewExpense = new EditExpenseViewModel
            {
                Id = expense.Id,
                ExpenseType = expense.ExpenseType.Name,
                Cost = expense.Price.ToString(),
                Date = expense.DateOfPayment,
                ExpenseTypes = expenseTypes,
            };

            return View(viewExpense);
        }

        [HttpPost]
        public IActionResult Edit(EditExpenseViewModel expense)
        {
            var price = this.DecimalValue(expense.Cost);

            if (price <= 0)
            {
                ModelState.AddModelError(string.Empty, "Платената сума трябва да е положително число");
            }

            if (expense.Date > DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Датата на плащане трябва да е преди текущата");
            }

            if (!ModelState.IsValid)
            {
                var expenses =  expensService.GetExpenseTypes(this.GetAddressId());
                expense.ExpenseTypes = expenses;
                return View(expense);
            }

            this.expensService.EditExpense(expense.Id,int.Parse(expense.ExpenseType),
                price, expense.Date, this.GetAddressId());

            return RedirectToAction(nameof(GetAll));
        }
    }
}
