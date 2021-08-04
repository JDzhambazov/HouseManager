namespace HouseManager.Web.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using HouseManager.Common;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class BaseController : Controller
    {
        public decimal DecimalValue(string value)
        {
            value = value.Replace(',', '.');
            decimal.TryParse(value, GlobalConstants.decimalStyle, CultureInfo.InvariantCulture, out decimal result);
            return result;
        }

        public int GetAddressId() 
            => int.Parse(this.Request.Cookies[GlobalConstants.AddressCookieName]);

        public void SetAddressId(int currentAddressId) 
            => this.Response.Cookies.Append(GlobalConstants.AddressCookieName, $"{currentAddressId}");
    
        public SelectList AddNullValueInSelectListItem(SelectList item)
        {
            SelectListItem selListItem = new SelectListItem() { Value = "null", Text = "Всички" };

            List<SelectListItem> newList = new List<SelectListItem>();

            newList.Add(selListItem);
            newList.AddRange(item);

            return new SelectList(newList, "Value", "Text", null);
        }
    }
}
