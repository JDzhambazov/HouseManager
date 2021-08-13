namespace HouseManager.Web.Tests.Pipeline
{
    using HouseManager.Services.Data.Models;
    using HouseManager.Web.Controllers;
    using HouseManager.Web.ViewModels.Expens;
    using MyTested.AspNetCore.Mvc;
    using System;
    using Xunit;

    using static Data.MyTestConstants;

    public class ExpenseControllerTest
    {
        [Fact]
        public void AddExpensetGetMethodMustReturnView()
            => MyMvc
            .Pipeline()
            .ShouldMap(request => request
            .WithPath("/Expense/AddExpense")
            .WithUser())
            .To<ExpenseController>(c => c.AddExpense())
            .Which(controller => controller
            .WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value))
            .WithUser())
            .ShouldReturn()
            .View(view => view
            .WithModelOfType<ExpenseViewModel>());

        [Theory]
        [InlineData(1, "Почистване", "20", true)]
        public void AddPaymasterPostMustReturnBadRequestIfUserNotMakeChanges(
                int addressId,
                string expensType,
                string price,
                bool isRegular)
             => MyMvc
             .Pipeline()
             .ShouldMap(request => request
             .WithPath("/Expense/AddExpense")
             .WithMethod(HttpMethod.Post)
             .WithFormFields(new
             {
                 AddressId = 1,
                 ExpensType = expensType,
                 Price = price,
                 //Date = new DateTime(DateTime.Now.Year,
                 //    DateTime.Now.Month,
                 //    DateTime.Now.Day),
                 IsRegular = true,
             })
             .WithUser(user => user.InRole("Manager"))
             .WithAntiForgeryToken())
             .To<ExpenseController>(c => c.AddExpense(newExpense))
             .Which(c => c.WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value)))
            .ShouldHave()
            .ValidModelState()
            .AndAlso()
            .ShouldReturn()
            .Redirect(redirect => redirect
            .To<ExpenseController>(c => c.AddExpense()));
    }
}
