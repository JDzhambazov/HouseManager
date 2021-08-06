namespace HouseManager.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data;
    using HouseManager.Web.ViewModels;
    using HouseManager.Web.ViewModels.Addresses;
    using HouseManager.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAddressService addressService;

        public HomeController(IUserService userService, 
            UserManager<ApplicationUser> userManager, 
            IAddressService addressService)
        {
            this.userService = userService;
            this.userManager = userManager;
            this.addressService = addressService;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var indexInfo = new HomeViewModel();

                indexInfo.UsersAddresses = userService.GetUserAddresses(User.Identity.Name);
                indexInfo.CurrntAddress = userService.CurrentAddressInfo(this.GetAddressId());

                return this.View(indexInfo);
            }

            return this.View();
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

        public async Task<IActionResult> SetCurrentAddress(int id)
        {
            var user = await userManager.GetUserAsync(this.User);

            if (await addressService.SetCurrentAddressId(id, user))
            {
                this.SetAddressId(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
