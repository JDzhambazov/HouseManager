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
    using HouseManager.Web.ViewModels.DueAmount;

    public class DueAmountService : IDueAmountService
    {
        private readonly ApplicationDbContext db;
        private readonly IFeeService feeService;

        public DueAmountService(ApplicationDbContext db , IFeeService feeService)
        {
            this.db = db;
            this.feeService = feeService;
        }

        public void AddMounthDueAmountInProperies(int propertyId, int month, int year)
        {
            var property = this.db.Properties.FirstOrDefault(x => x.Id == propertyId);
            this.AddDueAmount(month, year, property);
        }

        public void AddMounthDueAmountInAllProperies(int addressId)
        {
            var properties = this.db.Properties.Where(x => x.AddressId == addressId).ToList();
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;

            foreach (var property in properties)
            {
                this.AddDueAmount(month, year, property);
            }
        }

        public void EditMountDueAmount(int month, int year, int propertyId, decimal cost, bool isRegular)
        {
            if (isRegular)
            {
                var currentAmount = this.db.RegularDueAmounts
                    .Where(x => x.Month == month && x.Year == year && x.PropertyId == propertyId)
                    .FirstOrDefault();
                currentAmount.Cost = cost;
            }
            else
            {
                var currentAmount = this.db.NotRegularDueAmounts
                    .Where(x => x.Month == month && x.Year == year && x.PropertyId == propertyId)
                    .FirstOrDefault();
                currentAmount.Cost = cost;
            }

            this.db.SaveChanges();
        }

        public (decimal RegularDueAmount, decimal NotRegularDueAmount) GetPropertyMountDueAmount(int propertyId)
        {
            var currentMonthDueAmaount = this.db.RegularDueAmounts
                .FirstOrDefault(x => x.PropertyId == propertyId && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year);

            if (currentMonthDueAmaount == null)
            {
                var property = this.db.Properties.FirstOrDefault(x => x.Id == propertyId);
                this.AddMounthDueAmountInAllProperies(property.Id);
            }

            var regularDueAmount = this.db.RegularDueAmounts
                .Where(x => x.PropertyId == propertyId)
                .Sum(x => x.Cost) - this.db.RegularIncomes
                .Where(x => x.PropertyId == propertyId)
                .Sum(x => x.Price);

            var notRegularDueAmount = this.db.NotRegularDueAmounts
                .Where(x => x.PropertyId == propertyId)
                .Sum(x => x.Cost) - this.db.NotRegularIncomes
                .Where(x => x.PropertyId == propertyId)
                .Sum(x => x.Price);

            return (regularDueAmount > 0 ? regularDueAmount : 0, notRegularDueAmount > 0 ? notRegularDueAmount : 0);
        }

        public void AddStartDueAmount(int propertyId, int month, int year, decimal cost, bool isRegular)
        {
            if (isRegular)
            {
                this.db.RegularDueAmounts.Add(new RegularDueAmount
                {
                    Year = year,
                    Month = month,
                    Cost = cost,
                    PropertyId = propertyId,
                });
            }
            else
            {
                this.db.NotRegularDueAmounts.Add(new NotRegularDueAmount
                {
                    Year = year,
                    Month = month,
                    Cost = cost,
                    PropertyId = propertyId,
                });
            }
        }

        public ICollection<MonthAmountViewModel> GetAddressDueAmount(ICollection<Property> properties)
        {
            var result = new List<MonthAmountViewModel>();

            foreach (var property in properties)
            {
                var dueAmount = this.GetPropertyMountDueAmount(property.Id);
                if (dueAmount.RegularDueAmount > 0 || dueAmount.NotRegularDueAmount > 0)
                {
                    var user = this.db.Properties
                        .Where(x => x.Id == property.Id)
                        .Select(x => x.Residents.FirstOrDefault())
                        .FirstOrDefault();
                    result.Add(new MonthAmountViewModel
                    {
                        Id = property.Id,
                        PropertyName = property.Name,
                        ResidentName = user.FullName,
                        ResidentsCount = property.ResidentsCount,
                        RegularDueAmount = dueAmount.RegularDueAmount,
                        NotRegularDueAmount = dueAmount.NotRegularDueAmount,
                    });
                }
            }

            return result;
        }

        private void AddDueAmount(int month, int year, Property property)
        {
            var propertyService = new PropertyService(this.db , this.feeService);
            var dueAmount = propertyService.CalculateDueAmount(property.Id);
            this.db.RegularDueAmounts.Add(new RegularDueAmount
            {
                Year = year,
                Month = month,
                Cost = dueAmount.RegularDueAmount,
                PropertyId = property.Id,
            });

            this.db.NotRegularDueAmounts.Add(new NotRegularDueAmount
            {
                Year = year,
                Month = month,
                Cost = dueAmount.NotRegularDueAmount,
                PropertyId = property.Id,
            });

            this.db.SaveChanges();
        }
    }
}
