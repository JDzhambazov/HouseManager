using HouseManager.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace HouseManager.Web.ViewModels.Addresses
{
    public class EditPropertyInputModel
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public PropertyType PropertyType { get; set; }

        [Range(0, 20)]
        public int ResidentsCount { get; set; }
    }
}
