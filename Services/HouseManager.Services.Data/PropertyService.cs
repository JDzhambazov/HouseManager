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
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext db;

        public PropertyService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddProperty(string name, string propertyType, int residents, int addressId)
        {
            var cyrrentPropertyType = this.db.PropertiesTypes.FirstOrDefault(x => x.Name == propertyType);
            this.db.Properties.Add(new Property
            {
                Name = name,
                PropertyType = cyrrentPropertyType ?? new PropertyType { Name = propertyType },
                AddressId = addressId,
                ResidentsCount = residents,
            });
            this.db.SaveChanges();
        }

        public void AddResidentToProperty(string propertyName, string userName, string firstName, string lastName, string email, string password, int addressId)
        {
            var currentProperty = this.db.Properties
                .Where(x => x.AddressId == addressId)
                .FirstOrDefault(x => x.Name == propertyName);
            var resident = this.db.Users.FirstOrDefault(x => x.Email == email)
                ?? new ApplicationUser
                {
                    UserName = userName,
                    FullName = string.Join(' ', firstName, lastName).Trim(),
                    NormalizedUserName = userName.ToUpper(),
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    CreatedOn = DateTime.Now,
                    PasswordHash = password,
                    EmailConfirmed = true,
                };
            currentProperty.Residents.Add(resident);
            this.db.SaveChanges();
        }

        public void AddResidentToProperty(string propertyName, string userName)
        {
            var currentProperty = this.db.Properties
                .FirstOrDefault(x => x.Name == propertyName);
            var resident = this.db.Users.FirstOrDefault(x => x.FullName == userName);
            currentProperty.Residents.Add(resident);
            this.db.SaveChanges();
        }

        public ICollection<ApplicationUser> GetAllResidents(int propertyId)
        {
            var result = this.db.Properties
                      .Where(x => x.Id == propertyId)
                      .Select(x => new
                      {
                          user = x.Residents,
                      })
                      .FirstOrDefault();
            return result.user.ToList();
        }

        public (decimal RegularDueAmount, decimal NotRegularDueAmount) CalculateDueAmount(int propertyId)
        {
            var fees = new FeeService(this.db);
            var property = this.db.Properties.FirstOrDefault(x => x.Id == propertyId);
            decimal regularDueAmount = 0;
            decimal notRegularDueAmount = 0;

            foreach (var item in fees.GetAllFeesInProperty(propertyId))
            {
                if (item.IsRegular && item.IsPersonal)
                {
                    regularDueAmount += item.Cost * property.ResidentsCount;
                }
                else if (item.IsRegular && !item.IsPersonal)
                {
                    regularDueAmount += item.Cost;
                }
                else if (!item.IsRegular && item.IsPersonal)
                {
                    notRegularDueAmount += item.Cost * property.ResidentsCount;
                }
                else
                {
                    notRegularDueAmount += item.Cost;
                }
            }

            return (regularDueAmount, notRegularDueAmount);
        }

        public void ChangeResidentsCount(int propertyId, int newResidentsCount)
        {
            var currentProperty = this.db.Properties.FirstOrDefault(x => x.Id == propertyId);
            currentProperty.ResidentsCount = newResidentsCount;
            this.db.SaveChanges();
        }

        public async Task<List<Property>> GetAllPropertiesInAddress(int addressId)
        {
            var result = db.Properties
                .Include(x => x.PropertyType)
                .Include(x => x.Residents)
                .Where(x => x.AddressId == addressId)
                .ToList();

            return result;
        }

        public async Task<Property> GetPropetyById(int id)
        {
            var propery = await db.Properties
                .Include(x => x.PropertyType)
                .Include(x => x.Residents)
                .FirstOrDefaultAsync(x => x.Id == id);

            return propery;
        }

        public async Task<bool> Edit(int id ,int residentsCount)
        {
            if (id == 0)
            {
                return false;
            }

            var currentProperty = await db.Properties.FirstOrDefaultAsync(x => x.Id == id);
            currentProperty.ResidentsCount = residentsCount;

            db.Entry(currentProperty).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return true;
        }
    }
}
