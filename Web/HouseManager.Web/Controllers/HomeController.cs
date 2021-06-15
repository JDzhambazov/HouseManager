namespace HouseManager.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data;
    using HouseManager.Web.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IUserService userService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IAddressService addressService;

        public HomeController(IUserService userService, SignInManager<ApplicationUser> signInManager, IAddressService addressService)
        {
            this.userService = userService;
            this.signInManager = signInManager;
            this.addressService = addressService;
        }

        public async Task<IActionResult> Index()
        {
            //var user = await signInManager.UserManager.GetUserAsync(User);
            //var name = user.Properties;
            var addresses = await userService.GetUserAddresses(User.Identity.Name);
            foreach (var address in addresses)
            {
                ReplaseNames(address);
            }
            return this.View(addresses);
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

        public IActionResult Test(int id)
        {
            var request = this.Request.Cookies.ContainsKey("CurrentAddressId");
            //
            if (!request)
            {

                this.Response.Cookies.Append(
                    "CurrentAddressId",
                    $"{id}");
            }
            else
            {
                var es = int.Parse(this.Request.Cookies["CurrentAddressId"]);
            }

            return RedirectToAction(nameof(Index));
        }

        private static void ReplaseNames(ViewModels.Addresses.AddressViewModel address)
        {
            if (address.CityName != null)
            {
                address.CityName = string.Concat("гр. " + address.CityName);
            }

            if (address.StreetName != null)
            {
                address.StreetName = string.Concat("ул. " + address.StreetName);
            }

            if (address.DistrictName != null)
            {
                address.DistrictName = string.Concat("ж.к. " + address.DistrictName);
            }

            if (address.Entrance != null)
            {
                address.Entrance = string.Concat("вх. " + address.Entrance);
            }

            if (address.Number != null)
            {
                if(address.DistrictName != null)
                {
                    address.Number = string.Concat("бл.№ " + address.Number);
                }
                else
                {
                    address.Number = string.Concat("№ " + address.Number);
                }
            }
        }
    }
}
