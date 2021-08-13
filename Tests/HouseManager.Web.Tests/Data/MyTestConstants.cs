namespace HouseManager.Web.Tests.Data
{
    using HouseManager.Common;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data.Models;
    using HouseManager.Web.ViewModels.Expens;
    using System;

    public static class MyTestConstants
    {
        public static Address Address
        => new Address
        {
            City = new City { Name = "CityTest" },
            District = new District { Name = "DistrictTest"},
            Street = new Street { Name = "StreetTest" },
        };

        public static (string Name, string Value) Cookie
        => (Name: GlobalConstants.AddressCookieName, Value: "1");

        public static AdressServiseInputModel AddNewAddress
        => new AdressServiseInputModel
        {
            City = "TestCity",
            Street = "TestStreet",
            District = "TestDistrict",
            Entrance = "3",
            Number = "12",
            NumberOfProperties = 1,
        };

        public static ExpenseViewModel newExpense
            => new ExpenseViewModel
            {
                AddressId = 0,
                ExpensType = "Почистване",
                Price ="20",
                //Date = new DateTime(DateTime.Now.Year,
                //    DateTime.Now.Month,
                //    DateTime.Now.Day),
                IsRegular = false,
            };
    }
}
