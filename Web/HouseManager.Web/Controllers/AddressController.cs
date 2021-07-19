namespace HouseManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HouseManager.Services.Data;
    using HouseManager.Web.ViewModels.Addresses;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AddressController : Controller
    {
        private readonly IPropertyService propertyService;

        public AddressController(IPropertyService propertyService)
        {
            this.propertyService = propertyService;
        }

        public async Task<IActionResult> GetAllProperies()
        {
            var currentAddress =int.Parse(this.Request.Cookies["CurrentAddressId"]);
            var properies = await propertyService.GetAllPropertiesInAddress(currentAddress);

            return View(properies);
        }

        public async Task<IActionResult> EditProperty(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await propertyService.GetPropetyById(int.Parse(id.ToString()));
            if (property == null)
            {
                return NotFound();
            }

            var currentProprty = new EditPropertyViewModel
            {
                Id = property.Id,
                Name=property.Name,
                PropertyType = property.PropertyType,
                PropertyTypeId = property.PropertyTypeId,
                ResidentsCount = property.ResidentsCount,
                Residents = property.Residents,
            };

            return View(currentProprty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProperty(EditPropertyInputModel property)
        {
            if (await propertyService.GetPropetyById(property.Id) == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            if(await propertyService.Edit(property.Id, property.ResidentsCount))
            {
                return RedirectToAction(nameof(GetAllProperies));
            }
            return View(property);
        }
    }
}
