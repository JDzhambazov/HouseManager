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
        public ActionResult AddIncome(AddIncomeViewModel income)
        {
            if (ModelState.IsValid)
            {
                var resident = dbContext.Users.FirstOrDefault(x => x.Id == income.Resident);
                var addressId = dbContext.Properties.FirstOrDefault(x => x.Id == income.PropertyId).AddressId;

                if (income.RegularIncome != null)
                {
                    var regularIncome = decimal.Parse(income.RegularIncome, GlobalConstants.decimalStyle, CultureInfo.InvariantCulture);
                    if (regularIncome > 0)
                    {
                        incomeService.AddIncome(income.PropertyId, regularIncome, income.RegularIncomeDate, resident, addressId, true);
                    }
                }


                if (income.NotRegularIncome != null)
                {
                    var notRegularIncome = decimal.Parse(income.NotRegularIncome, GlobalConstants.decimalStyle, CultureInfo.InvariantCulture);
                    if(notRegularIncome > 0)
                    {
                        incomeService.AddIncome(income.PropertyId, notRegularIncome, income.NotRegularIncomeDate, resident, addressId, false);
                    }
                }
                return Redirect("/DueAmount/MonthAmount");
            }
            return View(income);
        }
    }
}
