namespace HouseManager.Services.Models
{
    using System.Collections.Generic;

    public class PagingServiceModel<T>
    {
        public ICollection<T> ItemList { get; set; }

        public PagingModel Paging { get; set; } = new PagingModel();
    }
}
