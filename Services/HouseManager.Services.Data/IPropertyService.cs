namespace HouseManager.Services.Data
{
    using HouseManager.Data.Models;
    using HouseManager.Services.Data.Models;
    using HouseManager.Services.Models;
    using HouseManager.Web.ViewModels.Property;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPropertyService
    {
        void AddProperty(CreatePropertyServiceModel newProperty, int addressId);

        void AddResidentToProperty(int propertyId, string userName, string firstName, string lastName, string email, string password, int addressId);

        SelectList GetAllResidents(int propertyId);

        (decimal RegularDueAmount, decimal NotRegularDueAmount) CalculateDueAmount(int propertyId);

        void ChangeResidentsCount(int propertyId, int newResidentsCount);

        public PagingServiceModel<AllPropertyViewModel> GetAllPropertiesInAddress(int addressId, int page);

        public SelectList GetPropertiesInAddress(int addressId);

        public Task<Property> GetPropetyById(int id);

        public SelectList GetPropertyTypes();

        public Task<bool> Edit(EditPropertySevriceModel property);
    }
}
