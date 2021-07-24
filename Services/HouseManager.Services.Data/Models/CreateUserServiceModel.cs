namespace HouseManager.Services.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreateUserServiceModel
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

        [DataType(DataType.Password)]
        [Display(Name = "Потвърди паролата")]
        [Compare("Password", ErrorMessage = "Въведените пароли не са еднакви.")]
        public string ConfirmPassword { get; set; }
    }
}
