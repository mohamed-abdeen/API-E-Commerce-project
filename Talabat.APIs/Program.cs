using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();


            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var loggerfactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var context = services.GetRequiredService<StoreContext>();
                await context.Database.MigrateAsync(); //Update-Database

               
                await StoreContextSeed.SeedAsync(context, loggerfactory);
              
                var identitycontext = services.GetRequiredService<AppIdentityDbContext>();
                await identitycontext.Database.MigrateAsync(); //Update-Database
              
                var usermanger = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(usermanger);
            }
            catch (Exception ex)
            {
                var logger = loggerfactory.CreateLogger<Program>();

                logger.LogError(ex, "An Error Occured during apply migaration");
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
