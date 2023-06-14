using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Identity;

namespace Talabat.APIs.Extentions
{
    public static class IdentityServicesExtention
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services ,IConfiguration configuration )
        {
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // options.Password.RequiredLength = 6;
                //options.Password.RequireNonAlphanumeric = true; 
            })
                .AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme*/ options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme; 
            }
            )
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                    ValidateIssuer=true,
                    ValidIssuer= configuration["jwt:ValidIssuer"],
                    ValidateAudience= true,
                     ValidAudience = configuration["jwt:ValidAudience"],
                     ValidateLifetime=true,
                     ValidateIssuerSigningKey=true,
                     IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:Key"]))

                    };
                }

                );
            return services;
        }
    }
}
