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
    using HouseManager.Services.Models;
    using HouseManager.Services.Mapping;
    using HouseManager.Web.ViewModels.Incomes;

    public class IncomeService : IIncomeService
    {
        private readonly ApplicationDbContext db;
        private readonly IPagingService<AllIncomeServiceModel> pagingService;

        public IncomeService(ApplicationDbContext db,
            IPagingService<AllIncomeServiceModel> pagingService)
        {
            this.db = db;
            this.pagingService = pagingService;
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

        public void DeleteIncome(int incomeId , int addressId)
        {
            var regularIncome = db.RegularIncomes
                .FirstOrDefault(x => x.Id == incomeId && x.AddressId == addressId);
            var notRegularIncome = db.NotRegularIncomes
                .FirstOrDefault(x => x.Id == incomeId && x.AddressId == addressId);

            if(regularIncome != null)
            {
                this.db.RegularIncomes.Remove(regularIncome);
            }


            if (notRegularIncome != null)
            {
                this.db.NotRegularIncomes.Remove(notRegularIncome);
            }

            this.db.SaveChanges();
        }

        public PagingServiceModel<AllIncomeServiceModel> GetAll(int addressId, int page,
            string propertyId, DateTime startDate, DateTime endDate)
        {
            var incomes = new List<AllIncomeServiceModel>();
            var regularIncomesQuery = this.db.RegularIncomes
                .Where(x => x.AddressId == addressId);
            var notRegularIncomesQuery = this.db.NotRegularIncomes
                .Where(x => x.AddressId == addressId);

            if(propertyId != null && propertyId != "null")
            {
                var id = int.Parse(propertyId);
                regularIncomesQuery = regularIncomesQuery
                    .Where(x => x.PropertyId == id);
                notRegularIncomesQuery = notRegularIncomesQuery
                    .Where(x => x.PropertyId == id);
            }

            if (startDate > DateTime.MinValue && startDate < DateTime.Now)
            {
                regularIncomesQuery = regularIncomesQuery
                    .Where(x => x.Date >= startDate);
                notRegularIncomesQuery = notRegularIncomesQuery
                    .Where(x => x.Date >= startDate);
            }

            if (endDate > DateTime.MinValue && endDate < DateTime.Now)
            {
                regularIncomesQuery = regularIncomesQuery
                    .Where(x => x.Date <= endDate);
                notRegularIncomesQuery = notRegularIncomesQuery
                    .Where(x => x.Date <= endDate);
            }

            var regularInkocoms = regularIncomesQuery
                .Select(x => new AllIncomeServiceModel
                {
                    Id = x.Id,
                    PropertyId = x.PropertyId,
                    PropertyName = x.Property.Name,
                    Date = x.Date,
                    Price = x.Price,
                    IncomeName = "Общи части",
                    ResidentFullName = x.Resident.FullName ?? "N/A",
                })
                .ToList();

            var notRegularInkocoms = notRegularIncomesQuery
                .Select(x => new AllIncomeServiceModel
                {
                    Id = x.Id,
                    PropertyId = x.PropertyId,
                    PropertyName = x.Property.Name,
                    Date = x.Date,
                    Price = x.Price,
                    IncomeName = "Ремонт Вход",
                    ResidentFullName = x.Resident.FullName ?? "N/A",
                })
                .ToList();

            incomes.AddRange(regularInkocoms);
            incomes.AddRange(notRegularInkocoms);
            incomes = incomes.OrderByDescending(x => x.Date).ToList();

            return this.pagingService.GetPageInfo(incomes, page);
        }

        public ICollection<AllIncomeServiceModel> GetAllIncomeForPropery(int propertyId, bool isRegular)
        {
            if (isRegular)
            {
                return this.db.RegularIncomes
                    .Where(x => x.PropertyId == propertyId)
                    .To<AllIncomeServiceModel>()
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
                    .To<AllIncomeServiceModel>()
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
