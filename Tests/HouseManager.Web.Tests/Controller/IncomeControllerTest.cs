namespace HouseManager.Web.Tests.Controller
{
    using HouseManager.Web.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class IncomeControllerTest
    {
        [Fact]
        public void IncomeControllerMastHaveAutorizeAtribute()
            => MyController<IncomeController>
            .Instance()
            .ShouldHave()
            .Attributes(attribute => attribute
            .RestrictingForAuthorizedRequests());

    }
}
