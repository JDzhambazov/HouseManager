﻿namespace HouseManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using HouseManager.Data;
    using HouseManager.Common;
    using HouseManager.Services.Data;
    using HouseManager.Web.ViewModels.DueAmount;
    using HouseManager.Web.ViewModels.Incomes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize]
    public class IncomeController : BaseController
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
            var result = new AddIncomeFormModel
            {
                PropertyId = id,
                NotRegularIncome = currentAmount.NotRegularDueAmount.ToString(),
                RegularIncome = currentAmount.RegularDueAmount.ToString(),
                RegularIncomeDate = DateTime.Now,
                NotRegularIncomeDate = DateTime.Now,
                Residents = residentSelectItems,
            };
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddIncome(AddIncomeFormModel income)
        {
            if (ModelState.IsValid)
            {
                var resident = dbContext.Users.FirstOrDefault(x => x.Id == income.Resident);

                if (income.RegularIncome != null)
                {
                    income.RegularIncome = income.RegularIncome.Replace(',', '.');
                    decimal.TryParse(income.RegularIncome, GlobalConstants.decimalStyle, CultureInfo.InvariantCulture, out decimal regularIncome);
                    if (regularIncome > 0)
                    {
                        incomeService.AddIncome(income.PropertyId, regularIncome, income.RegularIncomeDate, resident, this.GetAddressId(), true);
                    }
                }

                if (income.NotRegularIncome != null)
                {
                    income.NotRegularIncome = income.NotRegularIncome.Replace(',', '.');
                    decimal.TryParse(income.NotRegularIncome, GlobalConstants.decimalStyle, CultureInfo.InvariantCulture, out decimal notRegularIncome);
                    if(notRegularIncome > 0)
                    {
                            incomeService.AddIncome(income.PropertyId, notRegularIncome, income.NotRegularIncomeDate, resident, this.GetAddressId(), false);     
                    }
                }
                return Redirect($"/DueAmount/MonthAmount/{this.GetAddressId()}");
            }
            return View(income);
        }
    }
}
