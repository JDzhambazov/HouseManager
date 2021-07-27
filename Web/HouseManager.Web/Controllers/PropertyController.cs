namespace HouseManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HouseManager.Services.Data;
    using HouseManager.Services.Data.Models;
    using HouseManager.Web.ViewModels.Property;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class PropertyController : BaseController
    {
        private readonly IPropertyService propertyService;
        private readonly IAddressService addressService;

        public PropertyController(
            IPropertyService propertyService,
            IAddressService addressService
            )
        {
            this.propertyService = propertyService;
            this.addressService = addressService;
        }

        public IActionResult Create() => this.View(this.NewProperty());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatePropertyServiceModel property)
        {
            var newProperty = this.NewProperty();
            if(newProperty.PropertyCount -1 > 0)
            {
                newProperty.Name = property.Name;
                newProperty.ResidentsCount = property.ResidentsCount;
                this.CreateProperty(property);
                return View(newProperty);
            }

            this.CreateProperty(property);
            return View(property);
            //return RedirectToAction(nameof(Index));
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
                Name = property.Name,
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

            if (await propertyService.Edit(property.Id, property.ResidentsCount))
            {
                return RedirectToAction(nameof(GetAllProperies));
            }
            return View(property);
        }

        public IActionResult Details(int? id)
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            return this.View();
        }

        public async Task<IActionResult> GetAllProperies()
        {
            var properies = await propertyService.GetAllPropertiesInAddress(this.GetAddressId());

            return View(properies);
        }

        private void CreateProperty(CreatePropertyServiceModel property)
        {
            this.propertyService.AddProperty(property , this.GetAddressId());
        }

        private CreatePropertyServiceModel NewProperty()
        {
            var property = new CreatePropertyServiceModel();
            property.MonthFees = this.addressService.GetAddressMounthFees(this.GetAddressId());
            property.PropertyTypes = this.propertyService.GetPropertyTypes();
            property.PropertyCount = this.addressService.GetPropertyCount(this.GetAddressId());

            return property;
        }
    }
}
