namespace HouseManager.Web.Tests.Pipeline
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
        public void IndexPageView()
            => MyMvc
                .Pipeline()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index())
                .Which()
                .ShouldReturn()
                .View();


        [Fact]
        public void ErrorPageView()
        => MyMvc
            .Pipeline()
            .ShouldMap("/Home/Error")
            .To<HomeController>(c => c.Error())
            .Which()
            .ShouldReturn()
            .View();

        [Fact]
        public void PrivacyPageView()
        => MyMvc
            .Pipeline()
            .ShouldMap("/Home/Privacy")
            .To<HomeController>(c => c.Privacy())
            .Which()
            .ShouldReturn()
            .View();

        //[Fact]
        //public void SetUrrentAddressShouldReturnRedirect()
        //    => MyMvc
        //    .Pipeline()
        //    .ShouldMap("/Home/SetCurrentAddress?id=1")
        //    .To<HomeController>(c => c.SetCurrentAddress(1))
        //    .Which(controller => controller
        //    .WithUser())
        //    .ShouldReturn()
        //    .Redirect(redirect => redirect
        //    .To<HomeController>(c => c.Index()));
    }
}
