using System.Linq;
using Microsoft.AspNetCore.Http;

namespace electricalstore.Extentions
{
    public static class ClaimExtentions
    {
        public static string GetUserId(this HttpContext httpContext){
            if (httpContext.User is null)
                return string.Empty;

            return httpContext.User.Claims.Single(x => x.Type == "id").Value;
        }
    }
}