namespace HouseManager.Web.ViewModels.Incomes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EditIncomeViewModel
    {
        public int Id { get; set; }

        public int? PropertyId { get; set; }

        [Display(Name ="Имот")]
        public string PropertyName { get; set; }

        [Required]
        [Display(Name = "Стойност")]
        public string Price { get; set; }

        [Display(Name = "Дата на плащане")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Вид плащане")]
        public string IncomeName { get; set; }

        [Required]
        [Display(Name = "Заплатил")]
        public string ResidentId { get; set; }

        public SelectList Residents { get; set; }
    }
}
