namespace HouseManager.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class District
    {
        public District()
        {
            this.Address = new HashSet<Address>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public ICollection<Address> Address { get; set; }
    }
}
