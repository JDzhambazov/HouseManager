namespace HouseManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using HouseManager.Common;
    using HouseManager.Services.Data.Models;

    public class PagingService<T> : IPagingService<T>
    {
        public PagingServiceModel<T> GetPageInfo(ICollection<T> collection, int page)
        {
            var pageInfo = new PagingServiceModel<T>();
            var maxRowPerPage = GlobalConstants.MaxRowPerPage;

            var currentPage = page > 0 ? page : 1;
            var maxPage = (int)Math.Ceiling((double)collection.Count / maxRowPerPage);

            pageInfo.CurrentPage = currentPage > maxPage ? maxPage : currentPage;
            pageInfo.MaxPages = maxPage;
            pageInfo.ItemList = collection.Skip((currentPage - 1) * maxRowPerPage).Take(maxRowPerPage).ToList();
            
            return pageInfo;
        }
    }
}
