namespace HouseManager.Services.Data
{
    using System.Collections.Generic;
    using HouseManager.Services.Data.Models;

    public interface IPagingService<T>
    {
        public PagingServiceModel<T> GetPageInfo(ICollection<T> collection, int currentPage);
    }
}
