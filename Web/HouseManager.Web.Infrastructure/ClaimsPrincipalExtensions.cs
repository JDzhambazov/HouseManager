using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HouseManager.Web.Infrastructure
{

    public static class ClaimsPrincipalExtensions
    {
        public static int CurrentAddresId(this ClaimsPrincipal user , HttpContext context)
        => int.Parse(context.Request.Cookies["CurrentAddressId"]);
    }
}
