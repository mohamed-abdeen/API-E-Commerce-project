using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "mohamed abdeen ",
                    Email = "mohamed.abdeen@gmail.com",
                    UserName = "mohamed.abdeen",
                    PhoneNumber = "0123456789",

                };
                await userManager.CreateAsync(user,"pa$$w0rd");
            }
        }
    }
}
