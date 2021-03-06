namespace HouseManager.Web.ViewModels.Property
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using HouseManager.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EditPropertyViewModel
    {
        [Range(1,int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public string PropertyTypeName { get; set; }

        public SelectList PropertyTypes { get; set; }

        [Range(0,20)]
        public int ResidentsCount { get; set; }

        public DateTime StartDate { get; set; }
    }
}
