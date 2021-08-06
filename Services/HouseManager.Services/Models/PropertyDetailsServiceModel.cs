namespace HouseManager.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class PropertyDetailsServiceModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public int PropertyTypeId { get; set; }

        public string PropertyTypeName { get; set; }

        [MaxLength(2)]
        public int ResidentsCount { get; set; }

        public IEnumerable<UserEmailAndFullname> Residents { get; set; }

        public IEnumerable<string> MonthFees { get; set; }
    }
}
