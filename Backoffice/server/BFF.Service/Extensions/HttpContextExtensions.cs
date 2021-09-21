using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BFF.Service.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext context)
        {
            return context.User.FindFirstValue(Services.AuthService.IdClaim);
        }
        
        public static string GetUsername(this HttpContext context)
        {
            return context.User.FindFirstValue(Services.AuthService.UsernameClaim);
        }
    }
}