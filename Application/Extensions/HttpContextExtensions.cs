using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Application.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetUserId(this HttpContext httpContext)
        {
            var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? httpContext.User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(id))
                throw new UnauthorizedAccessException("User ID claim is missing.");

            return Guid.Parse(id);
        }
    }
}
