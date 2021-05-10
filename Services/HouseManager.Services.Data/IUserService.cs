namespace HouseManager.Services.Data
{
    using System;

    public interface IUserService
    {
        void AddNewUser(string userName, string firstName, string lastName, string email, string password);
    }
}
