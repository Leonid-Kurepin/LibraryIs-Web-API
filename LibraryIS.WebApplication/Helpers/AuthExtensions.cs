using System.Security.Claims;
using System.Text;
using LibraryIS.Auth;
using LibraryIS.CommonLayer.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LibraryIS.WebApplication.Helpers
{
    public static class AuthExtensions
    {
        public static void ConfigureAppSettingsObjects(this IServiceCollection services, IConfiguration configuration)
        {
            // configure strongly typed settings objects
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
        }

        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // configure jwt authentication
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        public static UserDto GetAuthorizedUser(this ControllerBase apiController)
        {
            var authorizedUser = new UserDto
            {
                Name = apiController.User.FindFirst(ClaimTypes.GivenName)?.Value,
                Email = apiController.User.FindFirst(ClaimTypes.Email)?.Value,
            };

            return authorizedUser;
        }

    }
}
