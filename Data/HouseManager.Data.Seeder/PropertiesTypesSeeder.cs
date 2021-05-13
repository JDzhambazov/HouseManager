namespace HouseManager.Data.Seeder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HouseManager.Data.Models;

    internal class PropertiesTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.PropertiesTypes.Any())
            {
                return;
            }
            var propertiesTypes = new List<PropertyType>();

            propertiesTypes.Add(new PropertyType { Name = "Апартамент" });
            propertiesTypes.Add(new PropertyType { Name = "Ателие" });
            propertiesTypes.Add(new PropertyType { Name = "Студио" });
            propertiesTypes.Add(new PropertyType { Name = "Къща" });
            propertiesTypes.Add(new PropertyType { Name = "Гараж" });
            propertiesTypes.Add(new PropertyType { Name = "Паркомясто" });

            await dbContext.PropertiesTypes.AddRangeAsync(propertiesTypes);
        }
    }
}
