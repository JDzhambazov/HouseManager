namespace HouseManager.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data;
    using HouseManager.Services.Messaging;
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
        private readonly IEmailSender emailSender;

        public HomeController(
            IAddressService addressService,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager,
            IUserService userService
            )
        {
            this.userService = userService;
            this.userManager = userManager;
            this.addressService = addressService;
            this.emailSender = emailSender;
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

        public async Task<IActionResult> SendEmail()
        {
            var message = new StringBuilder();
            message.AppendLine("<h1>Hello</h1>");
            message.AppendLine($"<h3>{DateTime.Now}</h3>");
            await this.emailSender.SendEmailAsync("jamby@mail.bg", "Ivan",
                "jamby@mail.bg", "Плащане на задължения", message.ToString());
            return RedirectToAction(nameof(Index));
        }
    }
}
