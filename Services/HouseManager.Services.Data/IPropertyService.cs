namespace HouseManager.Services.Data
{
    using HouseManager.Data.Models;
    using HouseManager.Services.Data.Models;
    using HouseManager.Web.ViewModels.Property;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPropertyService
    {
        void AddProperty(CreatePropertyServiceModel newProperty, int addressId);

        void AddResidentToProperty(string propertyName, string userName, string firstName, string lastName, string email, string password, int addressId);

        ICollection<ApplicationUser> GetAllResidents(int propertyId);

        (decimal RegularDueAmount, decimal NotRegularDueAmount) CalculateDueAmount(int propertyId);

        void ChangeResidentsCount(int propertyId, int newResidentsCount);

        public List<AllPropertyViewModel> GetAllPropertiesInAddress(int addressId);

        public Task<Property> GetPropetyById(int id);

        public List<SelectListItem> GetPropertyTypes();

        public Task<bool> Edit(int id, int residentsCount);
    }
}
