namespace HouseManager.Services.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class EditPropertySevriceModel
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public string PropertyTypeName { get; set; }

        public SelectList PropertyTypes { get; set; }

        [Range(0, 20)]
        public int ResidentsCount { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;
    }
}
