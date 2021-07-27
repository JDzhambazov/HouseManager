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
    using HouseManager.Web.ViewModels.Property;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext db;
        private readonly IFeeService feeService;

        public PropertyService(ApplicationDbContext db, IFeeService feeService)
        {
            this.db = db;
            this.feeService = feeService;
        }

        public void AddProperty(CreatePropertyServiceModel newProperty, int addressId)
        {
            var currentPropertyType = this.db
                .PropertiesTypes
                .FirstOrDefault(x => x.Name == newProperty.PropertyType);
            
            var property = new Property
            {
                Name = newProperty.Name,
                PropertyType = currentPropertyType ?? new PropertyType { Name = newProperty.PropertyType },
                AddressId = addressId,
                ResidentsCount = newProperty.ResidentsCount,
            };

            this.db.Properties.Add(property);
            this.db.SaveChanges();

            foreach (var fee in newProperty.Fees)
            {
                this.feeService.AddFeeToProperty(property.Id, fee);
            }
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

        public async Task<bool> Edit(int propertId, int residentsCount)
        {
            if (propertId == 0)
            {
                return false;
            }


            var currentProperty = await db.Properties.FirstOrDefaultAsync(x => x.Id == propertId);

            currentProperty.ResidentsCount = residentsCount;

            db.Entry(currentProperty).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return true;
        }

        public List<AllPropertyViewModel> GetAllPropertiesInAddress(int addressId)
        {
            var result = db.Properties
                .Include(x => x.PropertyType)
                .Include(x => x.Residents)
                .Where(x => x.AddressId == addressId)
                .Select(x => new AllPropertyViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PropertyType = x.PropertyType.Name,
                    ResidentsCount = x.ResidentsCount.ToString(),
                    Owner = x.Residents.Select(x => x.FullName).FirstOrDefault() ?? "N/A",
                })
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

        public List<SelectListItem> GetPropertyTypes()
            => this.db.PropertiesTypes.Select(x => new SelectListItem
            {
                Value = x.Name,
                Text = x.Name,
            }).ToList();
    }
}
