using System.Collections.Generic;

namespace HouseManager.Services.Data.Models
{

    public class MonthAmountServiseModel : PagingServiceModel
    {
        public IEnumerable<AmountListServiceModel> Amounts { get; set; }   
    }
}
