namespace HouseManager.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PropertyType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
    }
}
