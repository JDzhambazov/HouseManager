namespace HouseManager.Services.Data
{
    using System;
    using System.Collections;
    using HouseManager.Common;
    using HouseManager.Services.Data.Models;

    public class PagingService : IPagingService
    {
        public PagingServiceModel GetPageInfo(ICollection collection, int page)
        {
            var pageInfo = new PagingServiceModel();
            var maxRowPerPage = GlobalConstants.MaxRowPerPage;

            var currentPage = page > 0 ? page : 1;
            var maxPage = (int)Math.Ceiling((double)collection.Count / maxRowPerPage);

            pageInfo.CurrentPage = currentPage > maxPage ? maxPage : currentPage;
            pageInfo.MaxPages = maxPage;

            return pageInfo;
        }
    }
}
