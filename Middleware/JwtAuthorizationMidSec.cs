using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WebAdmin_KingAnttorny.Lib;

namespace API.KingAttorney.Middleware
{
    
    public  class JwtAuthorizationMidSec
    {
        private readonly RequestDelegate _next;

        public JwtAuthorizationMidSec(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var userModel = httpContext.Session.GetString("UserModel");
            if (userModel != null)
            {
                // Extract value from token and validate 
                var tokenObj = TokenProvider.DecodeToken(userModel);

                if (tokenObj != null && tokenObj.exp > DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                    httpContext.Items["Token"] = userModel;
            }
            return _next(httpContext);
        }
    }


    public static class JwtAuthorizationMidSecExtensions
    {
        public static IApplicationBuilder UseJwtAuthorizationMidSec(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtAuthorizationMidSec>();
        }
    }

}
