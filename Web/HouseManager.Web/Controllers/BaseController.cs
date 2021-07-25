namespace HouseManager.Web.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        public int GetAddressId() 
            => int.Parse(this.Request.Cookies["CurrentAddressId"]);

        public void SetAddressId(int currentAddressId) 
            => this.Response.Cookies.Append("CurrentAddressId", $"{currentAddressId}");

    }
}
