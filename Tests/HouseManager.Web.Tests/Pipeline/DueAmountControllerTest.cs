namespace HouseManager.Web.Tests.Pipeline
{
    using HouseManager.Services.Data.Models;
    using HouseManager.Services.Models;
    using HouseManager.Web.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using static Data.MyTestConstants;

    public class DueAmountControllerTest
    {
        [Fact]
        public void MonthAmountMustReturnView()
            => MyMvc
            .Pipeline()
            .ShouldMap(request => request
            .WithPath("/DueAmount/MonthAmount")
            .WithQueryString("?CurrentPage=2")
            .WithUser())
            .To<DueAmountController>(c => c.MonthAmount(2))
            .Which(controller => controller
            .WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value))
            .WithUser())
            .ShouldReturn()
            .View(view => view
            .WithModelOfType<PagingServiceModel<AmountListServiceModel>>());
    }
}
