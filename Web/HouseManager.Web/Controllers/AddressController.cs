namespace HouseManager.Web.Controllers
{
    using HouseManager.Services.Data;
    using HouseManager.Services.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Authorize]
    public class AddressController : Controller
    {
        private readonly IAddressService addressService;

        public AddressController(IAddressService addressService)
        {
            this.addressService = addressService;
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

            

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentAddressId = await this.addressService
                .CreateAddress(
                address.City,
                address.District,
                address.Street,
                address.Number,
                address.Entrance,
                address.NumberOfProperties,
                userId);

            this.Response.Cookies.Append("CurrentAddressId", $"{currentAddressId}");

            return RedirectToAction();
        }
    }
}
