namespace HouseManager.Web.Controllers
{
    using HouseManager.Common;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Globalization;

    public class BaseController : Controller
    {
        public int GetAddressId() 
            => int.Parse(this.Request.Cookies["CurrentAddressId"]);

        public void SetAddressId(int currentAddressId) 
            => this.Response.Cookies.Append("CurrentAddressId", $"{currentAddressId}");

        public decimal DecimalValue(string value)
        {
            value = value.Replace(',', '.');
            decimal.TryParse(value, GlobalConstants.decimalStyle, CultureInfo.InvariantCulture, out decimal result);
            return result;
        }
    }
}
