namespace HouseManager.Services.Data
{
    using HouseManager.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPropertyService
    {
        void AddProperty(string name, string propertyType, int residents, int addressId);

        void AddResidentToProperty(string propertyName, string userName, string firstName, string lastName, string email, string password, int addressId);

        ICollection<ApplicationUser> GetAllResidents(int propertyId);

        (decimal RegularDueAmount, decimal NotRegularDueAmount) CalculateDueAmount(int propertyId);

        void ChangeResidentsCount(int propertyId, int newResidentsCount);

        public Task<List<Property>> GetAllPropertiesInAddress(int addressId);

        public Task<Property> GetPropetyById(int id);

        public Task<bool> Edit(int id, int residentsCount);
    }
}
