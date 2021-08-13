namespace HouseManager.Web.Tests.Controller
{
    using HouseManager.Web.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class ExpenseControllerTest
    {
        [Fact]
        public void ExpenseControllerMastHaveAutorizeAtribute()
            => MyController<ExpenseController>
            .Instance()
            .ShouldHave()
            .Attributes(attribute => attribute
            .RestrictingForAuthorizedRequests());
    }
}
