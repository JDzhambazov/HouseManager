namespace HouseManager.Web.Controllers
{
    using System;
    using System.Globalization;
    using HouseManager.Common;
    using HouseManager.Services.Data;
    using HouseManager.Web.ViewModels.Expens;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ExpenseController : BaseController
    {
        private readonly IExpensService expensService;

        public ExpenseController(IExpensService expensService)
        {
            this.expensService = expensService;
        }

        public IActionResult AddExpense()
        {

            var result = new ExpenseViewModel
            {
                AddressId = this.GetAddressId(),
                ExpensTypeList = expensService.GetExpenseTypes(this.GetAddressId()),
                Date = DateTime.UtcNow,
            };

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddExpense(ExpenseViewModel expense)
        {
            var price = decimal.Parse(expense.Price, GlobalConstants.decimalStyle, CultureInfo.InvariantCulture);
            expensService.AddExpens(expense.ExpensType, price, expense.Date, expense.IsRegular, expense.AddressId);
            return RedirectToAction(nameof(AddExpense));
        }
    }
}
