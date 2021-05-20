using HouseManager.Common;
using HouseManager.Data;
using HouseManager.Data.Models;
using HouseManager.Services.Data;
using HouseManager.Web.ViewModels.Expens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HouseManager.Web.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IExpensService expensService;

        public ExpenseController(ApplicationDbContext dbContext, IExpensService expensService)
        {
            this.dbContext = dbContext;
            this.expensService = expensService;
        }
        public IActionResult AddExpense()
        {
            var expenseType = dbContext.ExpensesTypes.ToList();
            var expenseTypeSelectItems = new List<SelectListItem>();
            foreach (var item in expenseType)
            {
                expenseTypeSelectItems.Add(new SelectListItem { Value = item.Name, Text = item.Name });
            }

            var result = new ExpenseViewModel
            {
                AddressId = 1,
                ExpensTypeList = expenseTypeSelectItems,
                Date = DateTime.Now,
            };

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddExpense(ExpenseViewModel expense)
        {
            var price = decimal.Parse(expense.Price, GlobalConstants.decimalStyle, CultureInfo.InvariantCulture);
            expensService.AddExpens(expense.ExpensType, price, expense.Date, expense.IsRegular, expense.AddressId);
            return Redirect("/Expense/AddExpense");
        }
    }
}
