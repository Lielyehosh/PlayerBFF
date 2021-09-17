using Microsoft.AspNetCore.Http;

namespace BFF.Service.Extensions
{
    public static class ControllerExtensions
    {
        public static string GetCorrelationIdFromContext(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("x-correlation-id", out var header))
            {
                return header.ToString();
            }

            if (!context.Items.TryGetValue("corr", out var corr))
            {
                corr = System.Guid.NewGuid().ToString();
                context.Items["corr"] = corr;
                return (string) corr;
            }

            return null;
        }

        public static string GetCorrelationId(this HttpContext context)
        {
            return GetCorrelationIdFromContext(context);
        }
    }
}