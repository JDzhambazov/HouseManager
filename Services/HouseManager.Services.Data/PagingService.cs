namespace HouseManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using HouseManager.Common;
    using HouseManager.Services.Models;

    public class PagingService<T> : IPagingService<T>
    {
        public PagingServiceModel<T> GetPageInfo(ICollection<T> collection, int page)
        {
            var pageInfo = new PagingServiceModel<T>();
            var maxRowPerPage = GlobalConstants.MaxRowPerPage;

            var currentPage = page > 0 ? page : 1;
            var maxPage = (int)Math.Ceiling((double)collection.Count / maxRowPerPage);

            pageInfo.Paging.CurrentPage = currentPage > maxPage ? maxPage : currentPage;
            pageInfo.Paging.MaxPages = maxPage;
            pageInfo.ItemList = collection.Skip((currentPage - 1) * maxRowPerPage).Take(maxRowPerPage).ToList();
            
            if(pageInfo.Paging.MaxPages < 11)
            {
                pageInfo.Paging.StartPage = 1;
                pageInfo.Paging.EndPage = pageInfo.Paging.MaxPages;
            }
            else if (pageInfo.Paging.CurrentPage <= 6 )
            {
                pageInfo.Paging.StartPage = 1;
                pageInfo.Paging.EndPage = 11;
            }
            else
            {
                pageInfo.Paging.EndPage = pageInfo.Paging.CurrentPage + 5 > pageInfo.Paging.MaxPages 
                    ? pageInfo.Paging.MaxPages : pageInfo.Paging.CurrentPage + 5;
                pageInfo.Paging.StartPage = pageInfo.Paging.EndPage -10;
            }

            return pageInfo;
        }
    }
}
