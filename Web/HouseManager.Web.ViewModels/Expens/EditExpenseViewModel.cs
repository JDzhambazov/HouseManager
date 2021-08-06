namespace HouseManager.Web.ViewModels.Expens
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EditExpenseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Описание на разхода")]
        public string ExpenseType { get; set; }

        public SelectList ExpenseTypes { get; set; }

        [Required]
        [Display(Name = "Стойност")]
        public string Cost { get; set; }

        [Display(Name = "Дата на плащане")]
        public DateTime Date { get; set; }
    }
}
