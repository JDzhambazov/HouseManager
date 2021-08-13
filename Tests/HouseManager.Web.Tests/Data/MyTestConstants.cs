namespace HouseManager.Web.Tests.Data
{
    using HouseManager.Common;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data.Models;
    using HouseManager.Web.ViewModels.Expens;
    using HouseManager.Web.ViewModels.Incomes;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class MyTestConstants
    {
        public static AddIncomeFormModel AddIncome
            => new AddIncomeFormModel
            {
                PropertyId = 1,
                RegularIncome = "20",
                RegularIncomeDate = new DateTime(DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day),
                NotRegularIncome = "30",
                NotRegularIncomeDate = new DateTime(DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day),
                Resident = "TestId",
            };

        public static Address Address
        => new Address
        {
            City = new City { Name = "CityTest" },
            District = new District { Name = "DistrictTest" },
            Street = new Street { Name = "StreetTest" },
        };

        public static Address AddNewAddress
            => new Address
            {
                City = new City { Name = "TestCity" },
                Street = new Street { Name = "TestStreet" },
                District = new District { Name = "TestDistrict" },
                Entrance = "3",
                Number = "12",
                NumberOfProperties = 10,
            };

        public static CreatePropertyServiceModel CreateProperty
            => new CreatePropertyServiceModel
            {
                Name = "TestProp",
                PropertyType = "TestType",
                ResidentsCount = 3,
                PropertyCount = 2,
                //Fee = StringList,
            };

    public static readonly List<string> StringList =
            new List<string> { "test1", "test2", "test3" };

        public static (string Name, string Value) Cookie
        => (Name: GlobalConstants.AddressCookieName, Value: "1");


        public static ExpenseViewModel newExpense
        => new ExpenseViewModel
        {
            AddressId = 1,
            ExpensType = "Почистване",
            Price ="20",
            Date = new DateTime(DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day),
            IsRegular = true,
        };

        public static IEnumerable<Property> GetProperty
            => Enumerable.Range(1, 10).Select(x => new Property());


        public static ApplicationUser UserInDatabase
            => new ApplicationUser
            {
                Id = "TestId",
                UserName = "TestName",
                FullName = "TestName TestName",
                Email = "test@test.com",
            };
    }
}
