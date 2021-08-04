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
    using HouseManager.Web.ViewModels.Property;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext db;
        private readonly IFeeService feeService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPagingService<AllPropertyViewModel> pagingService;

        public PropertyService(ApplicationDbContext db, 
            IFeeService feeService,
            UserManager<ApplicationUser> userManager,
            IPagingService<AllPropertyViewModel> pagingService)
        {
            this.db = db;
            this.feeService = feeService;
            this.userManager = userManager;
            this.pagingService = pagingService;
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

        public void AddResidentToProperty(int propertyId, string userName, string firstName, string lastName, string email, string password, int addressId)
        {
            var currentProperty = this.db.Properties
                .FirstOrDefault(x => x.Id == propertyId);

            var resident = this.db.Users.FirstOrDefault(x => x.Email == email);
                
            if(resident == null)
            {
                var user = new ApplicationUser
                {
                    UserName = userName,
                    FullName = string.Join(" ", firstName, lastName).Trim(),
                    Email = email,
                };

                Task.Run(async () =>
                {
                   await this.userManager.CreateAsync(user, password);
                }).GetAwaiter().GetResult();
                currentProperty.Residents.Add(user);
            }
            else
            {
                currentProperty.Residents.Add(resident);
            }

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

        public SelectList GetPropertiesInAddress(int addressId)
        => new SelectList(this.db.Properties
                .Where(x => x.AddressId == addressId)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                }).ToList(), "Value", "Text", null);

        public SelectList GetAllResidents(int propertyId)
             => new SelectList(this.db.Properties
                .Where(x => x.Id == propertyId)
                .SelectMany(x => x.Residents.Select(r =>
                new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.FullName,
                })).ToList(), "Value", "Text", null);
        //{
        //    var result = this.db.Properties
        //              .Where(x => x.Id == propertyId)
        //              .Select(x => new
        //              {
        //                  user = x.Residents,
        //              })
        //              .FirstOrDefault();
        //    return result.user.ToList();
        //}

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

        public PagingServiceModel<AllPropertyViewModel> GetAllPropertiesInAddress(int addressId, int page)
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

            return this.pagingService.GetPageInfo(result, page);
        }

        public async Task<Property> GetPropetyById(int id)
        {
            var propery = await db.Properties
                .Include(x => x.PropertyType)
                .Include(x => x.Residents)
                .FirstOrDefaultAsync(x => x.Id == id);

            return propery;
        }

        public SelectList GetPropertyTypes()
            => new SelectList(this.db.PropertiesTypes.Select(x => new SelectListItem
            {
                Value = x.Name,
                Text = x.Name,
            }).ToList(), "Value", "Text", null);
    }
}
