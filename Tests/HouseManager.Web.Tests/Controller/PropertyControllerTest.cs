namespace HouseManager.Web.Tests.Controller
{
    using HouseManager.Web.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class PropertyControllerTest
    {
        [Fact]
        public void PropertyControllerMastHaveAutorizeAtribute()
            => MyController<PropertyController>
            .Instance()
            .ShouldHave()
            .Attributes(attribute => attribute
            .RestrictingForAuthorizedRequests());
    }
}
