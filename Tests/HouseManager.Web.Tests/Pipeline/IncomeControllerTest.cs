namespace HouseManager.Web.Tests.Pipeline
{
    using HouseManager.Data.Models;
    using HouseManager.Web.Controllers;
    using HouseManager.Web.ViewModels.Incomes;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using Xunit;

    using static Data.MyTestConstants;

    public class IncomeControllerTest
    {

        [Fact]
        public void AddIncomeMustReturnView()
            => MyMvc
            .Pipeline()
            .ShouldMap(request => request
            .WithPath("/Income/AddIncome")
            .WithQueryString("?id=1")
            .WithUser())
            .To<IncomeController>(c => c.AddIncome(1))
            .Which(controller => controller
            .WithData(GetProperty)
            .WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value)
            )
            .WithUser())
            .ShouldReturn()
            .View(view => view
            .WithModelOfType<AddIncomeFormModel>());

        [Theory]
        [InlineData("1", "20", "30", "TestId")]
        public void AddIncomesPostMustRedirectToAddDueAmountMonthAmount(
            string propertyId ,
            string regularIncome ,
            string notRegularIncome ,
             string resident)
             => MyMvc
             .Pipeline()
             .ShouldMap(request => request
             .WithPath("/Income/AddIncome")
             .WithMethod(HttpMethod.Post)
             .WithFormFields(new
             {
                 PropertyId = propertyId,
                 RegularIncome = regularIncome,
                 RegularIncomeDate = new DateTime(DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day).ToString(),
                 NotRegularIncome = notRegularIncome,
                 NotRegularIncomeDate = new DateTime(DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day).ToString(),
                 Resident = resident,

             })
             .WithUser(user => user.InRole("Manager"))
             .WithAntiForgeryToken())
             .To<IncomeController>(c => c.AddIncome(AddIncome))
             .Which(c => c
             .WithData(UserInDatabase)
             .WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value)))
            .ShouldHave()
            .ValidModelState()
            .AndAlso()
            .ShouldReturn()
            .Redirect();
    }
}
