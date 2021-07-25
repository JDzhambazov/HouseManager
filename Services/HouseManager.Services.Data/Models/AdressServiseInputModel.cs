using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseManager.Services.Data.Models
{
    public class AdressServiseInputModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Населено място")]
        public string City { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Комплекс(ж.к.)")]
        public string District { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Улица")]
        public string Street { get; set; }

        [Required]
        [StringLength(5, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Номер")]
        public string Number { get; set; }

        [StringLength(5, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Вход")]
        public string Entrance { get; set; }

        [Required]
        [Display(Name = "Брой имоти(апартаменти,гаражи, ателиета и др.)")]
        [Range(1, int.MaxValue)]
        public int NumberOfProperties { get; set; } = 1;

        public IEnumerable<SelectListItem> AllCities { get; set; }

        public IEnumerable<SelectListItem> AllDistricts { get; set; }

        public IEnumerable<SelectListItem> AllStreets { get; set; }
    }
}
