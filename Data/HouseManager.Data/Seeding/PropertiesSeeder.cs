namespace HouseManager.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HouseManager.Data.Models;

    internal class PropertiesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {

            if (dbContext.Properties.Any())
            {
                return;
            }


            var properties = new List<Property>();

            properties.Add(new Property { Name = "Aп.17", PropertyTypeId = 1, ResidentsCount = 3, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.18", PropertyTypeId = 1, ResidentsCount = 3, AddressId = 1, });
            properties.Add(new Property { Name = "Aп.19", PropertyTypeId = 1, ResidentsCount = 1, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.20", PropertyTypeId = 1, ResidentsCount = 1, AddressId = 1 });
            properties.Add(new Property
            {
                Name = "Aп.21",
                PropertyTypeId = 1,
                ResidentsCount = 1,
                AddressId = 1,
            });
            properties.Add(new Property
            {
                Name = "Aп.22",
                PropertyTypeId = 1,
                ResidentsCount = 2,
                AddressId = 1,
            });
            properties.Add(new Property
            {
                Name = "Aп.23",
                PropertyTypeId = 1,
                ResidentsCount = 1,
                AddressId = 1,
            });
            properties.Add(new Property { Name = "Aп.24", PropertyTypeId = 1, ResidentsCount = 3, AddressId = 1 });
            properties.Add(new Property
            {
                Name = "Aп.25",
                PropertyTypeId = 1,
                ResidentsCount = 2,
                AddressId = 1,
            });
            properties.Add(new Property
            {
                Name = "Aп.26",
                PropertyTypeId = 1,
                ResidentsCount = 0,
                AddressId = 1,
            });
            properties.Add(new Property
            {
                Name = "Aп.27",
                PropertyTypeId = 1,
                ResidentsCount = 0,
                AddressId = 1,
            });
            properties.Add(new Property
            {
                Name = "Aп.28",
                PropertyTypeId = 1,
                ResidentsCount = 3,
                AddressId = 1,
            });
            properties.Add(new Property
            {
                Name = "Aп.29",
                PropertyTypeId = 1,
                ResidentsCount = 2,
                AddressId = 1,
            });
            properties.Add(new Property
            {
                Name = "Aп.30",
                PropertyTypeId = 1,
                ResidentsCount = 2,
                AddressId = 1,
            });
            properties.Add(new Property
            {
                Name = "Aп.31",
                PropertyTypeId = 1,
                ResidentsCount = 2,
                AddressId = 1,
            });
            properties.Add(new Property
            {
                Name = "Aп.32",
                PropertyTypeId = 1,
                ResidentsCount = 2,
                AddressId = 1,
            });

           await dbContext.Properties.AddRangeAsync(properties);
        }
    }
}
