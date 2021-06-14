namespace HouseManager.Web.Areas.Administration.Controllers
{
    using HouseManager.Services.Data;
    using HouseManager.Web.ViewModels.Administration.Dashboard;

    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { SettingsCount = 0, };
            return this.View(viewModel);
        }
    }
}
