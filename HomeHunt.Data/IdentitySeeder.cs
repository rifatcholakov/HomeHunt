using HomeHunt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HomeHunt
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services, string adminEmail, string adminPassword)
        {
            RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<IdentityUser> userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { Roles.Administrator, Roles.Agent, Roles.User };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            IdentityUser? admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);

                if (!result.Succeeded) {
                    throw new InvalidOperationException(
                        Messages.User.AdminAccountCreateFailedMessage + string.Join(", ", result.Errors.Select(e => e.Description))
                    );
                }
            }

            if(!await userManager.IsInRoleAsync(admin, Roles.Administrator))
            {
                await userManager.AddToRoleAsync(admin, Roles.Administrator);
            }
        }
    }
}
