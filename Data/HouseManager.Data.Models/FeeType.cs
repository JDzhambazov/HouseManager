namespace HouseManager.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class FeeType
    {
        public FeeType()
        {
            this.MonthFees = new HashSet<MonthFee>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public virtual ICollection<MonthFee> MonthFees { get; set; }
    }
}
