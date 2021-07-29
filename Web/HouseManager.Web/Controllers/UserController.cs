namespace HouseManager.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data;
    using HouseManager.Services.Data.Models;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IPropertyService propertyService;

        public UserController(
            IPropertyService propertyService,
            IUserService userService,
            SignInManager<ApplicationUser> signInManager)

        {
            this.userService = userService;
            this.signInManager = signInManager;
            this.propertyService = propertyService;
        }

        public IActionResult AddToProperty()
        {
            var resident = new AddUserToPropertyServiceModel();
            resident.Properties = this.propertyService.GetPropertiesInAddress(this.GetAddressId());
            return View(resident);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToProperty(AddUserToPropertyServiceModel user)
        {
            if (!ModelState.IsValid)
            {
                user.Properties =
                    this.propertyService.GetPropertiesInAddress(this.GetAddressId());
                return View(user);
            }

            foreach (var property in user.Property)
            {
                this.propertyService.AddResidentToProperty(
                    int.Parse(property),
                    user.UserName,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Password,
                    this.GetAddressId());
            }

            if (RouteData.Values["id"] != null)
            {
                return RedirectToAction(nameof(HomeController.Index),"Home");
            }

            return RedirectToAction(nameof(AddToProperty));
        }
    }
}
