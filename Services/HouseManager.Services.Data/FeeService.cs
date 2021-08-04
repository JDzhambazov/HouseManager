﻿namespace HouseManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using HouseManager.Data;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class FeeService : IFeeService
    {
        private readonly ApplicationDbContext db;

        public FeeService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddFeeToAddress(int addressId, string feeName, decimal cost, bool isPersonel, bool isRegular)
        {
            var currentFee = this.db.FeeTypes.FirstOrDefault(x => x.Name.ToUpper() == feeName.ToUpper())
                ?? new FeeType { Name = feeName };
            var address = this.db.Addresses.FirstOrDefault(x => x.Id == addressId);

            var fee = new MonthFee
            {
                Address = address,
                FeeType = currentFee,
                Cost = cost,
                IsPersonal = isPersonel,
                IsRegular = isRegular,
            };

            this.db.MonthlyFees.Add(fee);
            this.db.SaveChanges();
        }

        public void AddFeeToProperty(int propertyId, string feeName)
        {
            var property = this.db.Properties.FirstOrDefault(x => x.Id == propertyId);
            var currentFee = this.db.MonthlyFees.FirstOrDefault(x => x.FeeType.Name == feeName);

            property.MonthFees.Add(currentFee);

            this.db.SaveChanges();
        }

        public void EditAddresFee(int addressId, string feeName, decimal cost)
        {
            var fee = this.db.Addresses
                .Include(x => x.MonthlyFees)
                .ThenInclude(x => x.FeeType)
                .Select(x => x.MonthlyFees.FirstOrDefault(f => f.FeeType.Name == feeName))
                .FirstOrDefault();

            fee.Cost = cost;
            this.db.SaveChanges();
        }

        public SelectList GetAllFees()
        => new SelectList(this.db.FeeTypes.Select(f => new SelectListItem
        {
            Value = f.Name,
            Text = f.Name,
        }).ToList(),"Value", "Text", null);

        public ICollection<MonthFee> GetAllFeesInAddress(int addressId)
        {
            var fees = this.db.MonthlyFees
                .Include(x => x.Address)
                .Include(f => f.FeeType)
                .Where(x => x.AddressId == addressId)
                .ToList();
            return fees;
        }

        public ICollection<MonthFee> GetAllFeesInProperty(int propertyId)
        {
            var fees = this.db.Properties
                .Where(x => x.Id == propertyId)
                .Include(x => x.MonthFees)
                .ThenInclude(x => x.FeeType)
                .Select(x => x.MonthFees)
                .FirstOrDefault();
            return fees;
        }
    }
}
