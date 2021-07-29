namespace HouseManager.Services.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AddPaymasterSevriseModel :IRoleToAddress
    {
        public string UserId { get; set; }

        public string Title { get; set; } = "Касиер";

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
