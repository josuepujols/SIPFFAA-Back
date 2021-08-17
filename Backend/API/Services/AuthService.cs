using API.Interfaces;
using API.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using   Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Services
{
    public static class AuthService
    {
        public static IServiceCollection GetAuthService(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = configuration.GetSection("SecretKey").Value;
            var simmetricKey = System.Text.ASCIIEncoding.ASCII.GetBytes(secretKey);

            TokenValidationParameters validationParameters = new TokenValidationParameters 
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(simmetricKey),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            services.AddScoped<IAuthRepository, AuthRepository>();

            var authenticationBuilder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt => opt.TokenValidationParameters = validationParameters);
            
            return services;
        }      
    }
}