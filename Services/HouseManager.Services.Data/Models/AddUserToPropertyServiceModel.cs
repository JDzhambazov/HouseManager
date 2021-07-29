namespace HouseManager.Services.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AddUserToPropertyServiceModel:IValidatableObject
    {
        [Required]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Потребитеското име трябва да е мин {1} символ и максимум {0}")]
        [Display(Name = "Потребитеско име")]
        public string UserName { get; set; }

        [Required]
        [StringLength(15,MinimumLength =2,ErrorMessage ="Името трябва да е мин {1} символ и максимум {0}")]
        [Display(Name ="Собствено име")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Фамилията трябва да е мин {1} символ и максимум {0}")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "{0} трябжа да е между {2} и {1} символа.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Имот(имоти)")]
        public ICollection<string> Property { get; set; }

        public IEnumerable<SelectListItem> Properties { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Property == null)
            {
               yield return new ValidationResult("Въведете поне един имот",
                new[] { nameof(Property) });
            }
        }
    }
}
