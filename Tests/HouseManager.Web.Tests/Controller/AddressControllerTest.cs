namespace HouseManager.Web.Tests.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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
