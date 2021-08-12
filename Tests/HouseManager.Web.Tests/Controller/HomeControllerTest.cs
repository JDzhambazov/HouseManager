namespace HouseManager.Web.Tests.Controller
{
    using HouseManager.Web.Controllers;
    using HouseManager.Web.ViewModels.Home;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using static Data.MyTestConstants;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexMastReturnViewWithModelIfUserIsAuthenticated()
            => MyController<HomeController>
            .Instance(instance => instance
            .WithHttpRequest(req => req.WithCookie(Cookie.Name,Cookie.Value))
            .WithData(data => data.WithEntities(Address))
            .WithUser())
            .Calling(c => c.Index())
            .ShouldReturn()
            .View(view => view
            .WithModelOfType<HomeViewModel>());
    }
}
