namespace HouseManager.Data.Seeder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HouseManager.Data.Models;
    using HouseManager.Services.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    internal class FeesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            {
                var feeManager = serviceProvider.GetRequiredService<FeeService>();

                await SeedFeesAsync(feeManager, dbContext);
            }
        }

        private static async Task SeedFeesAsync(FeeService feeManager, ApplicationDbContext db)
        {
            feeManager.AddFeeToAddress(1, "Общи части", 3, true, true);
            feeManager.AddFeeToAddress(1, "Асансьор", 3.50m, true, true);
            feeManager.AddFeeToAddress(1, "Ремонт вход", 0, false, false);
        
            var propertyList = new List<int>();
            var address = db.Addresses
                .Include(x => x.Properties)
                .FirstOrDefault(x => x.Id == 1);
        
            //foreach (var item in address.Properties)
            //{
            //    propertyList.Add(item.Id);
            //}

            //foreach (var id in propertyList)
            //{
            //    feeManager.AddFeeToProperty(id, "Общи части");
            //}

            //foreach (var id in propertyList)
            //{
            //    feeManager.AddFeeToProperty(id, "Ремонт вход");
            //}

       
            var lift = new List<int>() { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            foreach (var id in propertyList)
            {
                feeManager.AddFeeToProperty(id,2);
            }
        }
    }
}
