namespace HouseManager.Data.Seeder
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
            var PropertyTypeId = dbContext.PropertiesTypes.FirstOrDefault(x => x.Name == "Апартамент").Id;
            properties.Add(new Property { Name = "Aп.17", PropertyTypeId = PropertyTypeId, ResidentsCount = 3, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.18", PropertyTypeId = PropertyTypeId, ResidentsCount = 3, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.19", PropertyTypeId = PropertyTypeId, ResidentsCount = 1, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.20", PropertyTypeId = PropertyTypeId, ResidentsCount = 1, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.21", PropertyTypeId = PropertyTypeId, ResidentsCount = 1, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.22", PropertyTypeId = PropertyTypeId, ResidentsCount = 2, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.23", PropertyTypeId = PropertyTypeId, ResidentsCount = 1, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.24", PropertyTypeId = PropertyTypeId, ResidentsCount = 3, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.25", PropertyTypeId = PropertyTypeId, ResidentsCount = 2, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.26", PropertyTypeId = PropertyTypeId, ResidentsCount = 0, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.27", PropertyTypeId = PropertyTypeId, ResidentsCount = 0, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.28", PropertyTypeId = PropertyTypeId, ResidentsCount = 3, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.29", PropertyTypeId = PropertyTypeId, ResidentsCount = 2, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.30", PropertyTypeId = PropertyTypeId, ResidentsCount = 2, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.31", PropertyTypeId = PropertyTypeId, ResidentsCount = 2, AddressId = 1 });
            properties.Add(new Property { Name = "Aп.32", PropertyTypeId = PropertyTypeId, ResidentsCount = 2, AddressId = 1 });

            await dbContext.Properties.AddRangeAsync(properties);
        }
    }
}
