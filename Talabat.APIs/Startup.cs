using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Reposatories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
           

            services.AddDbContext<StoreContext>(options =>

            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            } );

            services.AddDbContext<AppIdentityDbContext>(options =>

            {
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConectiobs"));
            });

            ApplicationServicesExtentions.AddApplicationServices(services);

            services.AddSingleton<IConnectionMultiplexer>(s =>
            {
                var connection =ConfigurationOptions.Parse( Configuration.GetConnectionString("redis"));
                return ConnectionMultiplexer.Connect(connection);
            }



            );

            //services.AddScoped(typeof(IGenaricReposatiory<>), typeof(GenaricReposatiory<>));
            services.AddApplicationServices();
            services.AddSwaggerServices();
            services.AddIdentityServices(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("corspolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            }
            
            );
           // services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseswaggerDocumentation();
                //   app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("corspolicy");
            app.UseAuthentication();    
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
