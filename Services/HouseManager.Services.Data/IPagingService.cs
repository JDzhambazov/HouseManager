namespace HouseManager.Services.Data
{
    using System.Collections;
    using HouseManager.Services.Data.Models;

    public interface IPagingService
    {
        public PagingServiceModel GetPageInfo(ICollection collection, int currentPage);
    }
}
