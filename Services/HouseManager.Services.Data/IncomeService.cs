namespace HouseManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using HouseManager.Data;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data;
    using HouseManager.Services.Data.Models;
    using HouseManager.Services.Mapping;
    using HouseManager.Web.ViewModels.Incomes;

    public class IncomeService : IIncomeService
    {
        private readonly ApplicationDbContext db;

        public IncomeService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddIncome(int? properyId, decimal price, DateTime date, ApplicationUser resident, int addressId, bool isRegular)
        {
            if (isRegular)
            {
                this.db.RegularIncomes.Add(new RegularIncome
                {
                    PropertyId = properyId,
                    Date = date,
                    Price = price,
                    Resident = resident,
                    AddressId = addressId,
                });
            }
            else
            {
                this.db.NotRegularIncomes.Add(new NotRegularIncome
                {
                    PropertyId = properyId,
                    Date = date,
                    Price = price,
                    Resident = resident,
                    AddressId = addressId,
                });
            }

            this.db.SaveChanges();
        }

        public void EditMounthProperyIncome(int propertyId, int month, int year, decimal newPrice, bool isRegular)
        {
            if (isRegular)
            {
                var currentIncome = this.db.RegularIncomes
                    .FirstOrDefault(x => x.PropertyId == propertyId && x.Date.Month == month && x.Date.Year == year);
                currentIncome.Price = newPrice;
                this.db.SaveChanges();
            }
            else
            {
                var currentIncome = this.db.NotRegularIncomes
                    .FirstOrDefault(x => x.PropertyId == propertyId && x.Date.Month == month && x.Date.Year == year);
                currentIncome.Price = newPrice;
                this.db.SaveChanges();
            }
        }

        public ICollection<IncomeServiceModel> GetAllIncomeForPropery(int propertyId, bool isRegular)
        {
            if (isRegular)
            {
                return this.db.RegularIncomes
                    .Where(x => x.PropertyId == propertyId)
                    .To<IncomeServiceModel>()
                    //.Select(x => new IncomeViewModel
                    //{
                    //    PropertyId = x.PropertyId,
                    //    Property = x.Property,
                    //    Date = x.Date,
                    //    Price = x.Price,
                    //    Resident = x.Resident,
                    //    ResidentId = x.ResidentId,
                    //})
                    .ToList();
            }
            else
            {
                return this.db.NotRegularIncomes
                    .Where(x => x.PropertyId == propertyId)
                    .To<IncomeServiceModel>()
                    //.Select(x => new IncomeViewModel
                    //{
                    //    PropertyId = x.PropertyId,
                    //    Property = x.Property,
                    //    Date = x.Date,
                    //    Price = x.Price,
                    //    Resident = x.Resident,
                    //    ResidentId = x.ResidentId,
                    //})
                    .ToList();
            }
        }
    }
}
