namespace HouseManager.Services.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using HouseManager.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CreatePropertyServiceModel
    {
        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public string PropertyType { get; set; }

        [Range(0, 20)]
        public int ResidentsCount { get; set; }

        public int PropertyCount { get; set; }

        [Required]
        public IEnumerable<string> Fees { get; set; }

        public IEnumerable<SelectListItem> PropertyTypes { get; set; }

        public IEnumerable<SelectListItem> MonthFees { get; set; }
    }
}
