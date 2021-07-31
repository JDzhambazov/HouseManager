namespace HouseManager.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HouseManager.Services.Data;
    using HouseManager.Services.Data.Models;
    using HouseManager.Web.Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize]
    public class AddressController : BaseController
    {
        private readonly IAddressService addressService;
        private readonly IFeeService feeService;
        private readonly IUserService userService;
        private readonly string manager = "Домоуправител";
        private readonly string payMaster = "Касиер/Счетоводител";

        public AddressController(
            IAddressService addressService,
            IFeeService feeService,
            IUserService userService)
        {
            this.addressService = addressService;
            this.feeService = feeService;
            this.userService = userService;
        }

        public IActionResult Create()
        {
            var address = this.addressService.GetSelectItems(new AdressServiseInputModel());

            return View(address);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdressServiseInputModel address)
        {
            if (address.City.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Името на населеното място трябва да е поне 3 символа");
            }

            if (address.District != null && address.District.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Името на комплекса трябва да е поне 3 символа");
            }

            if (address.Street != null && address.Street.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Името на улицата трябва да е поне 3 символа");
            }

            if (address.Street == null && address.District == null)
            {
                ModelState.AddModelError(string.Empty, "Адреса трябва да има улица или комплекс");
            }

            if (!ModelState.IsValid)
            {
                this.addressService.GetSelectItems(address);
                return View(address);
            }

            var currentAddressId = await this.addressService
                .CreateAddress(
                address.City,
                address.District,
                address.Street,
                address.Number,
                address.Entrance,
                address.NumberOfProperties,
                this.User.Id());

            this.SetAddressId(currentAddressId);

            return RedirectToAction();
        }

        public IActionResult AddManager()
        {
            var user = new RoleToAddress();
            user.Title = manager;
            user.Users = this.UserList();
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles ="Manager , PayMaster")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddManager(RoleToAddress user)
        {
            if (this.userService.IsUserMakeChanges(this.User.Id(),this.GetAddressId()))
            {
                return BadRequest();
            }

            user.Users = this.UserList();

            if (!user.Users.Any(x => x.Value == user.UserId))
            {
                ModelState.AddModelError(string.Empty, "Моля изберете съществуващ потребител");
            }

            if (!ModelState.IsValid)
            {
                user.Title = manager;
                return View(user);
            }

            await this.addressService.SetAddressManager(this.GetAddressId(), user.UserId);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult AddPaymaster()
        {
            var user = new RoleToAddress();
            user.Title = payMaster;
            user.Users = this.UserList();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPaymaster(RoleToAddress user)
        {
            if (this.userService.IsUserMakeChanges(this.User.Id(), this.GetAddressId()))
            {
                return BadRequest();
            }

            user.Users = this.UserList();

            if (!user.Users.Any(x => x.Value == user.UserId))
            {
                ModelState.AddModelError(string.Empty, "Моля изберете съществуващ потребител");
            }

            if (!ModelState.IsValid)
            {
                user.Title = payMaster;
                return View(user);
            }

            await this.addressService.SetAddressPaymaster(this.GetAddressId(), user.UserId);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult MounthFee()
        {
            return this.View(this.NewFee());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MounthFee(FeeServiseModel fee)
        {
            if (!ModelState.IsValid)
            {
                fee.FeeTypes = this.feeService.GetAllFees();
                return this.View(fee);
            }

            this.feeService
                .AddFeeToAddress(
                this.GetAddressId(),
                fee.FeeType,
                this.DecimalValue(fee.Cost),
                fee.IsPersonal,
                fee.IsRegular);

            return this.View(this.NewFee());
        }

        private FeeServiseModel NewFee()
        {
            var newFee = new FeeServiseModel();
            newFee.FeeTypes = this.feeService.GetAllFees();
            return newFee;
        }

        private IEnumerable<SelectListItem> UserList()
            => this.userService.GetAllUsersInAddress(GetAddressId());
    }
}
