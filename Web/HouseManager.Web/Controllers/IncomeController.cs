namespace HouseManager.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using HouseManager.Services.Data;
    using HouseManager.Services.Messaging;
    using HouseManager.Web.Infrastructure;
    using HouseManager.Web.ViewModels.Incomes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class IncomeController : BaseController
    {
        private readonly IPropertyService propertyService;
        private readonly IDueAmountService dueAmountService;
        private readonly IIncomeService incomeService;
        private readonly IEmailSender emailSender;
        private readonly IUserService userService;

        public IncomeController(
            IDueAmountService dueAmountService,
            IIncomeService incomeService,
            IEmailSender emailSender,
            IPropertyService propertyService,
            IUserService userService)
        {
            this.propertyService = propertyService;
            this.dueAmountService = dueAmountService;
            this.incomeService = incomeService;
            this.emailSender = emailSender;
            this.userService = userService;
        }

        public ActionResult AddIncome(int id) 
        {
            var currentAmount = dueAmountService.GetPropertyMountDueAmount(id);
            var result = new AddIncomeFormModel
            {
                PropertyId = id,
                NotRegularIncome = currentAmount.NotRegularDueAmount.ToString(),
                RegularIncome = currentAmount.RegularDueAmount.ToString(),
                RegularIncomeDate = DateTime.Now,
                NotRegularIncomeDate = DateTime.Now,
                Residents = propertyService.GetAllResidents(id),
            };
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddIncome(AddIncomeFormModel income)
        {
            if (ModelState.IsValid)
            {
                var resident = userService.GetUserById(income.Resident);

                if (income.RegularIncome != null)
                {
                    var result = this.DecimalValue(income.RegularIncome);
                    
                    if (result > 0)
                    {
                        incomeService.AddIncome(income.PropertyId, result, income.RegularIncomeDate, resident, this.GetAddressId(), true);
                    }
                }

                if (income.NotRegularIncome != null)
                {
                    var result = this.DecimalValue(income.NotRegularIncome);

                    if (result > 0)
                    {
                        incomeService.AddIncome(income.PropertyId, result, income.NotRegularIncomeDate, resident, this.GetAddressId(), false);     
                    }
                }

                var sender = userService.GetUserById(this.User.Id());
                var message = incomeService
                    .IncomeConfirmationМessage(income.RegularIncome,
                    income.NotRegularIncome, resident.FullName);
                await this.emailSender.SendEmailAsync(sender.Email, 
                    sender.FullName, 
                    resident.Email, 
                    "Получено плащане", 
                    message);
                return Redirect($"/DueAmount/MonthAmount/{this.GetAddressId()}");
            }
            return View(income);
        }
    }
}
