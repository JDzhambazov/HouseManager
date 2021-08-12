namespace HouseManager.Web.Tests.Routing
{
    using HouseManager.Data.Models;
    using HouseManager.Web.Controllers;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void SetUrrentAddressShouldReturnRedirect()
            => MyRouting
            .Configuration()
            .ShouldMap("/Home/SetCurrentAddress?id=1")
            .To<HomeController>(c => c.SetCurrentAddress(1));
    }
}
