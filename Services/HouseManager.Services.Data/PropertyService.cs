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

        /// <summary>
        /// For Console Application Only.
        /// </summary>
        /// <param name="propertyName">Property Name.</param>
        /// <param name="userName">User full name.</param>
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
    }
}
