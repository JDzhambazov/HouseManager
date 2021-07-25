namespace HouseManager.Web.Controllers
{
    using System.Collections.Generic;
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

        public AddressController(IAddressService addressService, IFeeService feeService)
        {
            this.addressService = addressService;
            this.feeService = feeService;
        }

        public IActionResult Create()
        {
            var address = this.addressService.GetSelectItems(new AdressServiseInputModel());

            return View(address);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdressServiseInputModel address)
        {
            if(address.City.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Името на населеното място трябва да е поне 3 символа");
            }

            if(address.District != null && address.District.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Името на комплекса трябва да е поне 3 символа");
            }

            if (address.Street != null && address.Street.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Името на улицата трябва да е поне 3 символа");
            }

            if(address.Street == null && address.District == null)
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
    }
}
