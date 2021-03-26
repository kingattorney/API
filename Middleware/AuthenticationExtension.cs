using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.KingAttorney.Middleware
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration config)  
        {  
            var secret = config.GetSection("Jwt_Operation").GetSection("secretKey").Value;
            var iss = config.GetSection("Jwt_Operation").GetSection("iss").Value;

            var key = Encoding.ASCII.GetBytes(secret);  
            services.AddAuthentication(x =>  
            {  
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  
            })  
            .AddJwtBearer(token =>  
            {
                token.RequireHttpsMetadata = false;
                token.SaveToken = true;
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = false,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero,

                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,

                    ValidIssuer = iss,//"https://localhost:3017/",
                    ValidAudience = iss // "https://localhost:3017/"

                };  
            });  
  
            return services;  
        }  

    }
}
