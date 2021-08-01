namespace HouseManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HouseManager.Services.Data;
    using HouseManager.Services.Data.Models;
    using HouseManager.Web.ViewModels.Property;
    using HouseManager.Web.Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class PropertyController : BaseController
    {
        private readonly IPropertyService propertyService;
        private readonly IAddressService addressService;
        private readonly IUserService userService;

        public PropertyController(
            IPropertyService propertyService,
            IAddressService addressService,
            IUserService userService
            )
        {
            this.propertyService = propertyService;
            this.addressService = addressService;
            this.userService = userService;
        }

        public IActionResult Create()
        {
            var property = this.NewProperty();

            if (property.PropertyCount <= 0)
            {
                return RedirectToAction(nameof(GetAllProperies));
            }

            return this.View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatePropertyServiceModel property)
        {
            if (!ModelState.IsValid)
            {
                var errorProperty = this.NewProperty();
                errorProperty.Name = property.Name;
                errorProperty.ResidentsCount = property.ResidentsCount;
                return View(errorProperty);
            }

            this.CreateProperty(property);

            var newProperty = this.NewProperty();
            if(newProperty.PropertyCount > 0)
            {
                newProperty.Name = property.Name;
                newProperty.ResidentsCount = property.ResidentsCount;
                return View(newProperty);
            }
            else
            {
                this.CreateProperty(property);
                return RedirectToAction(nameof(GetAllProperies));
            }
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

        public IActionResult GetAllProperies([FromQuery]int currentPage)
        {
            var properies = propertyService
                .GetAllPropertiesInAddress(this.GetAddressId(),currentPage);
            ViewBag.IsMakeChanges = this.userService.IsUserMakeChanges(this.User.Id(),this.GetAddressId());
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
