namespace HouseManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using HouseManager.Data;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data.Models;
    using HouseManager.Services.Models;
    using HouseManager.Common;
    using Microsoft.EntityFrameworkCore;

    public class DueAmountService : IDueAmountService
    {
        private readonly ApplicationDbContext db;
        private readonly IFeeService feeService;
        private readonly IAddressService addressService;
        private readonly IPropertyService propertyService;
        private readonly IPagingService<AmountListServiceModel> pagingService;

        public DueAmountService(
            ApplicationDbContext db,
            IFeeService feeService,
            IAddressService addressService,
            IPropertyService propertyService,
            IPagingService<AmountListServiceModel> pagingService
            )
        {
            this.db = db;
            this.feeService = feeService;
            this.addressService = addressService;
            this.propertyService = propertyService;
            this.pagingService = pagingService;
        }

        public void AddMounthDueAmountInProperies(int propertyId, int month, int year)
        {
            var property = this.db.Properties.FirstOrDefault(x => x.Id == propertyId);
            this.AddDueAmount(month, year, property.Id);
        }

        public void AddMounthDueAmountInAllProperies(int addressId)
        {
            var properties = this.db.Properties.Where(x => x.AddressId == addressId).ToList();
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;

            foreach (var property in properties)
            {
                this.AddDueAmount(month, year, property.Id);
            }
        }


        public void EditPropertyMountDueAmount(int propertyId, DateTime startDate)
        {
            var regularAmounts = this.db.RegularDueAmounts
                .Where(x => x.PropertyId == propertyId)
                .Where(x => x.Month >= startDate.Month && x.Year == startDate.Year)
                .ToList();

            var notRegularAmounts = this.db.NotRegularDueAmounts
                .Where(x => x.PropertyId == propertyId)
                .Where(x => x.Month >= startDate.Month && x.Year == startDate.Year)
                .ToList();

            if ( startDate.Year < DateTime.Now.Year)
            {
                regularAmounts.AddRange(this.db.RegularDueAmounts
                    .Where(x => x.PropertyId == propertyId)
                    .Where(x => x.Year > startDate.Year)
                    .ToList());

                notRegularAmounts.AddRange(this.db.NotRegularDueAmounts
                    .Where(x => x.PropertyId == propertyId)
                    .Where(x => x.Year > startDate.Year)
                    .ToList());
            }

            foreach (var amount in regularAmounts)
            {
                var currentAmount = propertyService.CalculateDueAmount(amount.PropertyId);
                amount.Cost = currentAmount.RegularDueAmount;
                db.Entry(amount).State = EntityState.Modified;
            }

            foreach (var amount in notRegularAmounts)
            {
                var currentAmount = propertyService.CalculateDueAmount(amount.PropertyId);
                amount.Cost = currentAmount.NotRegularDueAmount;
                db.Entry(amount).State = EntityState.Modified;
            }

            db.SaveChanges();
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
                this.AddMounthDueAmountInAllProperies(property.AddressId);
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

        public PagingServiceModel<AmountListServiceModel> GetAddressDueAmount(int addressId, int page)
        {
            var amounts = new List<AmountListServiceModel>();
            var properties = this.addressService.GetAllProperyies(addressId);

            foreach (var property in properties)
            {
                var dueAmount = this.GetPropertyMountDueAmount(property.Id);
                if (dueAmount.RegularDueAmount > 0 || dueAmount.NotRegularDueAmount > 0)
                {
                    var residentName = "N/A";

                    var user = this.db.Properties
                        .Where(x => x.Id == property.Id)
                        .Select(x => x.Residents.FirstOrDefault())
                        .FirstOrDefault();

                    if(user != null)
                    {
                        residentName = user.FullName;
                    }

                    amounts.Add(new AmountListServiceModel
                    {
                        Id = property.Id,
                        PropertyName = property.Name,
                        ResidentName = residentName,
                        ResidentsCount = property.ResidentsCount,
                        RegularDueAmount = dueAmount.RegularDueAmount,
                        NotRegularDueAmount = dueAmount.NotRegularDueAmount,
                    });
                }
            }
            
            return this.pagingService.GetPageInfo(amounts, page); ;
        }

        private void AddDueAmount(int month, int year, int propertyId)
        {
            var dueAmount = this.propertyService.CalculateDueAmount(propertyId);
            this.db.RegularDueAmounts.Add(new RegularDueAmount
            {
                Year = year,
                Month = month,
                Cost = dueAmount.RegularDueAmount,
                PropertyId = propertyId,
            });

            this.db.NotRegularDueAmounts.Add(new NotRegularDueAmount
            {
                Year = year,
                Month = month,
                Cost = dueAmount.NotRegularDueAmount,
                PropertyId = propertyId,
            });

            this.db.SaveChanges();
        }
    }
}
