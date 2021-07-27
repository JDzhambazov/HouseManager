namespace HouseManager.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data;
    using HouseManager.Services.Data.Models;


    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserController(IUserService userService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.signInManager = signInManager;
        }
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserServiceModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var newUser = new ApplicationUser
            {
                UserName = user.UserName,
                FullName = $"{user.FirstName} {user.LastName}".Trim(),
                Email = user.Email,
            };

            var result =await this.userService
                .AddNewUser(
                user.UserName,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Password);
            
            if (result.Succeeded)
            {
                await this.signInManager.SignInAsync(newUser, isPersistent: false);
                return RedirectToAction(nameof(AddressController.Create), "Address");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(user);
        }
    }
}
