namespace HouseManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HouseManager.Services.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class DueAmountController : Controller
    {
        private readonly IDueAmountService dueAmountService;
        private readonly IAddressService addressService;

        public DueAmountController(IDueAmountService dueAmountService, IAddressService addressService)
        {
            this.dueAmountService = dueAmountService;
            this.addressService = addressService;
        }
        public ActionResult MonthAmount(int id)
        {
            var result = dueAmountService.GetAddressDueAmount(addressService.GetAllProperyies(id));

            return View(result);
        }
        // GET: DueAmountController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DueAmountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DueAmountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DueAmountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DueAmountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DueAmountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DueAmountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DueAmountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
