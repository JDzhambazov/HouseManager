namespace HouseManager.Web.Tests.Pipeline
{
    using HouseManager.Services.Data.Models;
    using HouseManager.Web.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using static Data.MyTestConstants;

    public class AddressControllerTest
    {
        [Fact]
        public void CreateActionMastReturnView()
         => MyMvc
            .Pipeline()
            .ShouldMap(request => request
            .WithPath("/Address/Create")
            .WithUser())
            .To<AddressController>(c => c.Create())
            .Which()
            .ShouldReturn()
            .View();

        //[Fact]
        //public void CreateBecomeShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel()
        //    => MyPipeline
        //    .Configuration()
        //    .ShouldMap(request => request
        //        .WithPath("/Address/Create")
        //        .WithMethod(HttpMethod.Post)
        //        .WithFormFields(AddNewAddress)
        //        .WithUser()
        //        .WithAntiForgeryToken())
        //    .To<AddressController>(c => c.Create(AddNewAddress))
        //    .Which()
        //    .ShouldReturn()
        //    .View();

        [Fact]
        public void AddManagerActionMastReturnView()
        => MyMvc
           .Pipeline()
           .ShouldMap(request => request
           .WithPath("/Address/AddManager")          
           .WithUser())
           .To<AddressController>(c => c.AddManager())
           .Which(c => c.WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value)))
           .ShouldReturn()
           .View(view => view.
                WithModelOfType<RoleToAddress>());

        [Fact]
        public void AddPaymasterActionMastReturnView()
        => MyMvc
           .Pipeline()
           .ShouldMap(request => request
           .WithPath("/Address/AddPaymaster")
           .WithUser())
           .To<AddressController>(c => c.AddPaymaster())
           .Which(c => c.WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value)))
           .ShouldReturn()
           .View(view => view.
                WithModelOfType<RoleToAddress>());

        [Fact]
        public void MounthFeeActionMastReturnView()
        => MyMvc
           .Pipeline()
           .ShouldMap(request => request
           .WithPath("/Address/MounthFee")
           .WithUser())
           .To<AddressController>(c => c.MounthFee())
           .Which()
           .ShouldReturn()
           .View(view => view.
                WithModelOfType<FeeServiseModel>());
    }
}
