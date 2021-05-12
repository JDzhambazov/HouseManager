namespace HouseManager.Web.Controllers
{
    using System.Diagnostics;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data;
    using HouseManager.Web.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IUserService userService;
        private readonly SignInManager<ApplicationUser> signInManager;

        public HomeController(IUserService userService, SignInManager<ApplicationUser> signInManager)
        {
            this.userService = userService;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return this.View(userService.GetUserAddresses(User.Identity.Name));
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
