using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Reposatories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.APIs.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ITokenService, TokenServices>();
            services.AddScoped(typeof(IBasketRepo), typeof(BasketRepo));
          
            
            services.AddAutoMapper(typeof(MappingProfiles));

            services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = actioncontext =>
                {
                    var errors = actioncontext.ModelState.Where(m => m.Value.Errors.Count() > 0)
                     .SelectMany(m => m.Value.Errors)
                     .Select(e => e.ErrorMessage)
                     .ToArray();

                    var errorresponse = new ApiValidationErrorResponse()
                    {
                        Error = errors
                    };
                    return new BadRequestObjectResult(errorresponse);
                };
            } );

            return services;
        }
    }
}
