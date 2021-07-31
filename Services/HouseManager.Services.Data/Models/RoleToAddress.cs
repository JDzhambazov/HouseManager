namespace HouseManager.Services.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class RoleToAddress
    {
        [Required]
        [Display(Name ="Потребител")]
        public string UserId { get; set; }

        public string Title { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
