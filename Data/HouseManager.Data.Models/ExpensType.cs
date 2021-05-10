namespace HouseManager.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ExpensType
    {
        public ExpensType()
        {
            this.Expenses = new HashSet<Expens>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Expens> Expenses { get; set; }
    }
}
