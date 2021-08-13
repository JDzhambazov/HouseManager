namespace HouseManager.Web.Tests.Routing
{
    using HouseManager.Services.Data.Models;
    using HouseManager.Web.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using static Data.MyTestConstants;
    
    public class AddressControllerTest
    {
        [Fact]
        public void MatchCreateAddressPostAction()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Address/Create")
                .WithMethod(HttpMethod.Post)
                .WithAntiForgeryToken())
            .To<AddressController>(c => c.Create(With.Any<AdressServiseInputModel>()));

    }
}
