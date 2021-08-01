using System.Collections.Generic;

namespace HouseManager.Services.Data.Models
{
    public class PagingServiceModel<T>: PagingModel
    {
        public ICollection<T> ItemList { get; set; }
    }
}
