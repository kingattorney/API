using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WebAdmin_KingAnttorny.Lib;
namespace API.KingAttorney.Middleware
{
    public class JwtAuthorizationMid
    {
        private readonly RequestDelegate _next;

        public JwtAuthorizationMid(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                // Extract value from token and validate 
                var tokenObj = TokenProvider.DecodeToken(token);

                if (tokenObj != null && tokenObj.exp > DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                httpContext.Items["Token"] = token;
            }
            return _next(httpContext);
        }
    }

    public static class JwtAuthorizationMidExtensions
    {
        public static IApplicationBuilder UseJwtAuthorizationMid(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtAuthorizationMid>();
        }
    }
}
