namespace HouseManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HouseManager.Data;
    using HouseManager.Services.Data;
    using HouseManager.Web.ViewModels.DueAmount;
    using HouseManager.Web.ViewModels.Incomes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize]
    public class IncomeController : Controller
    {
        private readonly IPropertyService propertyService;
        private readonly IDueAmountService dueAmountService;
        private readonly IIncomeService incomeService;
        private readonly ApplicationDbContext dbContext;

        public IncomeController(
            IPropertyService propertyService,
            IDueAmountService dueAmountService,
            IIncomeService incomeService,
            ApplicationDbContext dbContext)
        {
            this.propertyService = propertyService;
            this.dueAmountService = dueAmountService;
            this.incomeService = incomeService;
            this.dbContext = dbContext;
        }

        public ActionResult AddIncome(int id) 
        {
            var residents = propertyService.GetAllResidents(id);
            var residentSelectItems = new List<SelectListItem>();

            foreach (var item in residents)
            {
                residentSelectItems.Add(new SelectListItem { Value = item.Id, Text = item.FullName });
            }

            var currentAmount = dueAmountService.GetPropertyMountDueAmount(id);
            var result = new AddIncomeViewModel
            {
                PropertyId = id,
                NotRegularIncome = currentAmount.NotRegularDueAmount,
                RegularIncome = currentAmount.RegularDueAmount,
                RegularIncomeDate = DateTime.Now,
                NotRegularIncomeDate = DateTime.Now,
                Residents = residentSelectItems,
            };
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddIncome(AddIncomeViewModel income)
        {
            if (ModelState.IsValid)
            {
                var resident = dbContext.Users.FirstOrDefault(x => x.Id == income.Resident);

                if (income.RegularIncome > 0)
                {
                    // incomeService.AddIncome(income.PropertyId, income.RegularIncome, income.RegularIncomeDate, resident, 1, true);
                }

                if (income.NotRegularIncome > 0)
                {
                    // incomeService.AddIncome(income.PropertyId, income.NotRegularIncome, income.NotRegularIncomeDate, resident, 1, false);
                }
                return Redirect("/DueAmount/MonthAmount");
            }
            return View(income);
        }
    }
}
