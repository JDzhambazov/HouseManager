namespace HouseManager.Web.Tests.Controller
{
    using HouseManager.Web.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class AddressControllerTest
    {
        [Fact]
        public void AddressControllerMastHaveAutorizeAtributeTest()
            => MyController<AddressController>
            .Instance()
            .ShouldHave()
            .Attributes(attribute => attribute
            .RestrictingForAuthorizedRequests());
    }
}
