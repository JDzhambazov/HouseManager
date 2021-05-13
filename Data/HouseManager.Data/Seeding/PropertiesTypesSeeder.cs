namespace HouseManager.Data.Seeding
{
    using System;
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

            await dbContext.PropertiesTypes.AddAsync(new PropertyType { Name = "Апартамент" });
            await dbContext.PropertiesTypes.AddAsync(new PropertyType { Name = "Ателие" });
            await dbContext.PropertiesTypes.AddAsync(new PropertyType { Name = "Студио" });
            await dbContext.PropertiesTypes.AddAsync(new PropertyType { Name = "Къща" });
            await dbContext.PropertiesTypes.AddAsync(new PropertyType { Name = "Гараж" });
            await dbContext.PropertiesTypes.AddAsync(new PropertyType { Name = "Паркомясто" });
        }
    }
}
