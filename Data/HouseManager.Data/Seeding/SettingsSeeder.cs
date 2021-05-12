namespace HouseManager.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using HouseManager.Data.Models;

    internal class SettingsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Settings.Any())
            {
                return;
            }

            await dbContext.Settings.AddAsync(new Setting { Name = "Setting1", Value = "value1" });
            await dbContext.Addresses.AddAsync(new Address
            { 
                City = new City { Name = "Бургас" },
                District = new District { Name = "Лазур" },
                Number = "88",
                Entrance = "Б",
                NumberOfProperties = 16,
            });
            await dbContext.Addresses.AddAsync(new Address
            {
                City = new City { Name = "София" },
                District = new District { Name = "Студентски град" },
                Number = "59А",
                NumberOfProperties = 104,
            });
        }
    }
}
