namespace HouseManager.Services.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class FeeServiseModel
    {
        [Required]
        [MaxLength(30)]
        [Display(Name ="Описание")]
        public string FeeType { get; set; }

        [Required]
        [StringLength(6, ErrorMessage = "{0} трябва де е между  {2} и {1} символа.", MinimumLength = 1)]
        [Display(Name = "Стойност")]
        public string Cost { get; set; }

        [Display(Name = "Постоянен разход(ток/асансьор)")]
        public bool IsRegular { get; set; }

        [Display(Name = "Разход за всеки обитател")]
        public bool IsPersonal { get; set; }

        public IEnumerable<SelectListItem> FeeTypes { get; set; }
    }
}
