namespace HouseManager.Services.Data
{
    using System.Collections.Generic;

    public interface IPropertyService
    {
        void AddProperty(string name, string propertyType, int residents, int addressId);

        void AddResidentToProperty(string propertyName, string userName, string firstName, string lastName, string email, string password, int addressId);

        ICollection<string> GetAllResidents(int propertyId);

        (decimal RegularDueAmount, decimal NotRegularDueAmount) CalculateDueAmount(int propertyId);

        void ChangeResidentsCount(int propertyId, int newResidentsCount);
    }
}
