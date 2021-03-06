namespace HouseManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HouseManager.Data.Models;
    using HouseManager.Services.Data;
    using HouseManager.Services.Data.Models;
    using HouseManager.Services.Models;
    using HouseManager.Web.Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize]
    public class AddressController : BaseController
    {
        private readonly IAddressService addressService;
        private readonly IDueAmountService dueAmountService;
        private readonly IFeeService feeService;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly string manager = "Домоуправител";
        private readonly string payMaster = "Касиер/Счетоводител";

        public AddressController(
            IAddressService addressService,
            IDueAmountService dueAmountService,
            IFeeService feeService,
            RoleManager<ApplicationRole> roleManager,
            IUserService userService,
            UserManager<ApplicationUser> userManager)
        {
            this.addressService = addressService;
            this.dueAmountService = dueAmountService;
            this.feeService = feeService;
            this.roleManager = roleManager;
            this.userService = userService;
            this.userManager = userManager;
        }

        public IActionResult Create()
        {
            var address = this.addressService.GetSelectItems(new AdressServiseInputModel());

            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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


            var roleName = "Creator";

            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            var user = userService.GetUserById(this.User.Id());
            await userManager.AddToRoleAsync(user , roleName);

            this.SetAddressId(currentAddressId);
            await addressService.SetCurrentAddressId(currentAddressId, user);
            return RedirectToAction(nameof(HomeController.Index),"Home");
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
            if (!this.userService.IsUserMakeChanges(this.User.Id(),this.GetAddressId()))
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
            if (!this.userService.IsUserMakeChanges(this.User.Id(), this.GetAddressId()))
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

        public IActionResult EditMonthFee(int addresId)
        {
            var fees = GetAddressFees(addresId);

            return this.View(fees);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMonthFee(ICollection<int> feeId, ICollection<string> cost, DateTime startDate)
        {
            if (ModelState.IsValid)
            {
                var feeList = feeId.ToArray();
                var costList = cost.ToArray();

                for (int i = 0; i < feeList.Count(); i++)
                {
                    var newCost = this.DecimalValue(cost.ToArray()[i]);
                    feeService.EditFee(feeList[i], this.DecimalValue(costList[i]));
                }

                var properties = addressService.GetAllProperyies(this.GetAddressId());

                foreach (var property in properties)
                {
                    dueAmountService.EditPropertyMountDueAmount(property.Id, startDate);
                }

                return RedirectToAction(nameof(DueAmountController.MonthAmount), "DueAmount");
            }

            return View(GetAddressFees(this.GetAddressId()));
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

            return RedirectToAction(nameof(MounthFee));
        }

        private FeeServiseModel NewFee()
        {
            var newFee = new FeeServiseModel();
            newFee.FeeTypes = this.feeService.GetAllFees();
            return newFee;
        }

        private EditMonthFeeServiceModel GetAddressFees(int addresId)
        {
            var fees = new EditMonthFeeServiceModel();
            fees.AddressFees = feeService.GetAllFeesInAddress(addresId);
            fees.StartDate = DateTime.Now;
            return fees;
        }

        private IEnumerable<SelectListItem> UserList()
            => this.userService.GetAllUsersInAddress(GetAddressId());
    }
}
