namespace HouseManager.Web.Tests.Controller
{
    using HouseManager.Web.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class DueAmountControllerTest
    {
        [Fact]
        public void DueAmountControllerMastHaveAutorizeAtribute()
            => MyController<DueAmountController>
            .Instance()
            .ShouldHave()
            .Attributes(attribute => attribute
            .RestrictingForAuthorizedRequests());
    }
}
