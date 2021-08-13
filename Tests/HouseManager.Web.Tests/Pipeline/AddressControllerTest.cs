namespace HouseManager.Web.Tests.Pipeline
{
    using HouseManager.Data.Models;
    using HouseManager.Services.Data.Models;
    using HouseManager.Web.Controllers;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Linq;
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

        //[Theory]
        //[InlineData("TestCity", "TestStreet","TestDistrict", "12","3", 12)]
        //public void CreateBecomeShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(
        //    string city,
        //    string street,
        //    string district,
        //    string number,
        //    string entrance,
        //    int numberofproperties
        //    )
        //    => MyPipeline
        //    .Configuration()
        //    .ShouldMap(request => request
        //        .WithPath("/Address/Create")
        //        .WithMethod(HttpMethod.Post)
        //        .WithFormFields(new
        //        {
        //            City = city,
        //            Street = street,
        //            District = district,
        //            Number = number,
        //            Entrance = entrance,
        //            NumberOfProperties = numberofproperties,
        //        })
        //        .WithUser(TestUser.Identifier,TestUser.Username)
        //        .WithAntiForgeryToken())
        //    .To<AddressController>(c => c.Create(AddNewAddress))
        //    .Which()
        //    .ShouldHave()
        //    .ValidModelState()
        //    .AndAlso()
        //    .ShouldReturn()
        //    .Redirect(redirect => redirect
        //    .To<HomeController>(c => c.Index()));

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

        [Theory]
        [InlineData("TestId")]
        public void AddManagerPostMustReturnBadRequestIfUserNotMakeChanges(string userId)
            => MyMvc
            .Pipeline()
            .ShouldMap(request => request
            .WithPath("/Address/AddManager")
            .WithMethod(HttpMethod.Post)
            .WithFormFields(new
            {
                UserId = userId,
            })
            .WithUser(user => user.InRole("Manager"))
            .WithAntiForgeryToken())
            .To<AddressController>(c => c.AddManager(new RoleToAddress { UserId = userId }))
           .Which(c => c.WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value)))
            .ShouldReturn()
            .BadRequest();

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

        [Theory]
        [InlineData("TestId")]
        public void AddPaymasterPostMustReturnBadRequestIfUserNotMakeChanges(string userId)
         => MyMvc
         .Pipeline()
         .ShouldMap(request => request
         .WithPath("/Address/AddPaymaster")
         .WithMethod(HttpMethod.Post)
         .WithFormFields(new
         {
             UserId = userId,
         })
         .WithUser(user => user.InRole("Manager"))
         .WithAntiForgeryToken())
         .To<AddressController>(c => c.AddPaymaster(new RoleToAddress { UserId = userId }))
        .Which(c => c.WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value)))
         .ShouldReturn()
         .BadRequest();

        [Fact]
        public void MonthFeeActionMastReturnView()
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

        [Theory]
        [InlineData("Общи часи")]
        public void MonthFeePostActionMustReturnRedirect(string feeType)
         => MyMvc
             .Pipeline()
             .ShouldMap(request => request
             .WithPath("/Address/MounthFee")
             .WithMethod(HttpMethod.Post)
             .WithFormFields(new
             {
                 FeeType = feeType,
             })
             .WithUser(user => user.InRole("Manager"))
             .WithAntiForgeryToken())
             .To<AddressController>(c => c.MounthFee(new FeeServiseModel { FeeType = feeType }))
            .Which(c => c.WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value)))
             .ShouldReturn()
             .View(view => view
            .WithModelOfType<FeeServiseModel>());
    }
}
