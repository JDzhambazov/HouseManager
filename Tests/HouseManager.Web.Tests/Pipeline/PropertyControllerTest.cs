namespace HouseManager.Web.Tests.Pipeline
{
    using HouseManager.Services.Data.Models;
    using HouseManager.Services.Models;
    using HouseManager.Web.Controllers;
    using HouseManager.Web.ViewModels.Property;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using Xunit;

    using static Data.MyTestConstants;

    public class PropertyControllerTest
    {
        [Fact]
        public void CreateActionMustReturnView()
            => MyMvc
            .Pipeline()
            .ShouldMap(request => request
            .WithPath("/Property/Create")
            .WithUser())
            .To<PropertyController>(c => c.Create())
            .Which(controller => controller
            .WithData(AddNewAddress)
            .WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value)
            )
            .WithUser())
            .ShouldReturn()
            .View(view => view
            .WithModelOfType<CreatePropertyServiceModel>());

        [Theory]
        [InlineData("TestProp", "TestType", "3", "2")]
        public void CreatePostMustReturnViewIfModelStateIsInvalid(
                 string name ,
                 string propertyType ,
                 string residentsCount,
                 string propertyCount)
             => MyMvc
             .Pipeline()
             .ShouldMap(request => request
             .WithPath("/Property/Create")
             .WithMethod(HttpMethod.Post)
             .WithFormFields(new
             {
                 Name = name,
                 PropertyType = propertyType,
                 ResidentsCount = residentsCount,
                 PropertyCount = propertyCount,
                 //Fee = StringList,
             })
             .WithUser(user => user.InRole("Manager"))
             .WithAntiForgeryToken())
             .To<PropertyController>(c => c.Create(CreateProperty))
             .Which(c => c
             .WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value)))
            .ShouldHave()
            .InvalidModelState()
            .AndAlso()
            .ShouldReturn()
            .View();

        [Fact]
        public void EditPropertyMustReturnNotFoundWithIncorectData()
            => MyMvc
            .Pipeline()
            .ShouldMap(request => request
            .WithPath("/Property/EditProperty")
            .WithQueryString("?id=1")
            .WithUser())
            .To<PropertyController>(c => c.EditProperty(1))
            .Which(controller => controller
            .WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value)
            )
            .WithUser())
            .ShouldReturn()
            .NotFound();

        [Fact]
        public void DetailsMustReturnView()
            => MyMvc
            .Pipeline()
            .ShouldMap(request => request
            .WithPath("/Property/Details")
            .WithQueryString("?id=1")
            .WithUser())
            .To<PropertyController>(c => c.Details(1))
            .Which(controller => controller
            .WithData(GetProperty)
            .WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value)
            )
            .WithUser())
            .ShouldReturn()
            .View();

        [Fact]
        public void GetAllProprtiesMustReturnView()
            => MyMvc
            .Pipeline()
            .ShouldMap(request => request
            .WithPath("/Property/GetAllProperies")
            .WithQueryString("?Currentpage=1")
            .WithUser())
            .To<PropertyController>(c => c.GetAllProperies(1))
            .Which(controller => controller
            .WithData(GetProperty)
            .WithHttpRequest(req => req.WithCookie(Cookie.Name, Cookie.Value)
            )
            .WithUser())
            .ShouldReturn()
            .View(view => view
            .WithModelOfType<PagingServiceModel<AllPropertyViewModel>>());
    }
}
