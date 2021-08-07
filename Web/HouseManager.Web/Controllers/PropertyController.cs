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
    using HouseManager.Services.Models;

    [Authorize]
    public class PropertyController : BaseController
    {
        private readonly IAddressService addressService;
        private readonly IDueAmountService dueAmountService;
        private readonly IPropertyService propertyService;
        private readonly IUserService userService;

        public PropertyController(
            IAddressService addressService,
            IDueAmountService dueAmountService,
            IPropertyService propertyService,
            IUserService userService
            )
        {
            this.addressService = addressService;
            this.dueAmountService = dueAmountService;
            this.propertyService = propertyService;
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
                return RedirectToAction(nameof(Create));
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

            var currentProprty = new EditPropertySevriceModel
            {
                Id = property.Id,
                Name = property.Name,
                PropertyTypeName = property.PropertyType.Name,
                PropertyTypes = propertyService.GetPropertyTypes(),
                ResidentsCount = property.ResidentsCount,
            };

            return View(currentProprty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProperty(EditPropertySevriceModel property)
        {
            if (await propertyService.GetPropetyById(property.Id) == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                property.PropertyTypes = propertyService.GetPropertyTypes();
                return View(property);
            }

            if (await propertyService.Edit(property))
            {
                dueAmountService.EditMountDueAmount(property.Id, property.StartDate);
                return RedirectToAction(nameof(DueAmountController.MonthAmount), "DueAmount");
            }
            return View(property);
        }

        public IActionResult Details(int id)
        {
            ViewBag.IsUserMakeChanges = userService
                .IsUserMakeChanges(this.User.Id(), this.GetAddressId());
            var propertyInfo = propertyService.Details(id);

            return this.View(propertyInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(PropertyDetailsServiceModel property)
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
            property.Fees = this.addressService.GetAddressMounthFees(this.GetAddressId());
            property.PropertyTypes = this.propertyService.GetPropertyTypes();
            property.PropertyCount = this.addressService.GetPropertyCount(this.GetAddressId());

            return property;
        }
    }
}
