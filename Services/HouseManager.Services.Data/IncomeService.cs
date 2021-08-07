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

        public void EditIncome(int incomeId, decimal newPrice, DateTime date, string residentId, int addressId)
        {
            var regularIncome = this.db.RegularIncomes
                .FirstOrDefault(x => x.Id == incomeId && x.AddressId == addressId);

            if(regularIncome != null)
            {
                regularIncome.Price = newPrice;
                regularIncome.Date = date;
                regularIncome.ResidentId = residentId;
            }
            else
            {
                var notRegularIncome = this.db.NotRegularIncomes
                    .FirstOrDefault(x => x.Id == incomeId && x.AddressId == addressId);

                if(notRegularIncome != null)
                {
                    notRegularIncome.Price = newPrice;
                    notRegularIncome.Date = date;
                    notRegularIncome.ResidentId = residentId;
                }
            }

            this.db.SaveChanges();
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

        public string IncomeConfirmationМessage(string regularIncome, string notRegularIncome, string payer)
        {
            var message = new StringBuilder();
            message.AppendLine("<h1>Потвърждение за извършено плащане</h1>");
            message.AppendLine($"<h2>Днес {DateTime.Now} е получено плащане за:</h2>");

            if(regularIncome != null)
            {
                message.AppendLine($"<h3>Сума за разходи за общи части -  {regularIncome} лв.</h3>");
            }

            if(notRegularIncome != null)
            {
                message.AppendLine($"<h3>Сума за разходи за ремот и поддръжка -  {notRegularIncome} лв.</h3>");
            }

            message.AppendLine("<br />");
            message.AppendLine("<h3>Благодарим за извършеното плащане.</h3>");
            message.AppendLine("<h5>Лек ден.</h6>");

            return message.ToString();
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
                    IncomeName = "Ремонти",
                    ResidentFullName = x.Resident.FullName ?? "N/A",
                })
                .ToList();

            incomes.AddRange(regularInkocoms);
            incomes.AddRange(notRegularInkocoms);
            incomes = incomes.OrderByDescending(x => x.Date).ToList();

            return this.pagingService.GetPageInfo(incomes, page);
        }

        public EditIncomeServiceModel GetById(int incomeId, int addressId) {
            var regularIncome = db.RegularIncomes
                .Select(x => new EditIncomeServiceModel
                {
                    Id = x.Id,
                    PropertyId = x.PropertyId,
                    PropertyName = x.Property.Name,
                    Price = x.Price,
                    Date = x.Date,
                    ResidentId = x.ResidentId,
                    AddressId = x.AddressId,
                    IncomeName = "Общи части",
                })
                .FirstOrDefault(x => x.Id == incomeId && x.AddressId == addressId);

            var notRegularIncome = db.NotRegularIncomes
                .Select(x => new EditIncomeServiceModel
                {
                    Id = x.Id,
                    PropertyId = x.PropertyId,
                    PropertyName = x.Property.Name,
                    Price = x.Price,
                    Date = x.Date,
                    ResidentId = x.ResidentId,
                    AddressId = x.AddressId,
                    IncomeName = "Ремонт",
                })
                .FirstOrDefault(x => x.Id == incomeId && x.AddressId == addressId);

            if(regularIncome != null)
            {
                return regularIncome;
            }

            if (notRegularIncome != null)
            {
                return notRegularIncome;
            }

            return null;
        }
    }
}
