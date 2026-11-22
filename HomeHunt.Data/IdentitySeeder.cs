using HomeHunt.Common;
using HomeHunt.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeHunt.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            IConfiguration configuration = services.GetRequiredService<IConfiguration>();
            RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<IdentityUser> userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();

            await EnsureRolesExistAsync(roleManager);

            IdentityUser admin = await EnsureUserExistsAsync(
                userManager,
                configuration[ConfigKeys.AdminEmail]!,
                configuration[ConfigKeys.AdminPassword]!,
                Messages.Account.AdminCreateFailedMessage);
            await EnsureUserIsInRolesAsync(userManager, admin, Roles.Administrator);

            IdentityUser demoUser = await EnsureUserExistsAsync(
                userManager,
                configuration[ConfigKeys.DemoUserEmail]!,
                configuration[ConfigKeys.DemoUserPassword]!,
                Messages.Account.DemoAccountCreateFailedMessage);
            await EnsureUserIsInRolesAsync(userManager, demoUser, Roles.User);

            await SeedDemoAgentsWithPropertiesAsync(userManager, context, configuration[ConfigKeys.DemoAgentPassword]!);
        }

        private static async Task EnsureRolesExistAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { Roles.Administrator, Roles.Agent, Roles.User };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task<IdentityUser> EnsureUserExistsAsync(
            UserManager<IdentityUser> userManager,
            string email,
            string password,
            string failureMessage)
        {
            IdentityUser? user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                return user;
            }

            user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            IdentityResult result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    failureMessage + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return user;
        }

        private static async Task EnsureUserIsInRolesAsync(UserManager<IdentityUser> userManager, IdentityUser user, params string[] roles)
        {
            foreach (string role in roles)
            {
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }

        private static async Task SeedDemoAgentsWithPropertiesAsync(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context,
            string demoAgentPassword)
        {
            Dictionary<int, int> agentIds = new Dictionary<int, int>();
            HashSet<int> createdKeys = new HashSet<int>();

            foreach (DemoAgent demo in SeedData.GetDemoAgents())
            {
                IdentityUser user = await EnsureUserExistsAsync(
                    userManager,
                    demo.Email,
                    demoAgentPassword,
                    Messages.Account.DemoAccountCreateFailedMessage);
                await EnsureUserIsInRolesAsync(userManager, user, Roles.User, Roles.Agent);

                Agent? agent = await context.Agents.FirstOrDefaultAsync(a => a.UserId == user.Id);

                if (agent == null)
                {
                    agent = new Agent
                    {
                        FirstName = demo.FirstName,
                        LastName = demo.LastName,
                        Phone = demo.Phone,
                        AgencyName = demo.AgencyName,
                        UserId = user.Id
                    };

                    context.Agents.Add(agent);
                    await context.SaveChangesAsync();

                    createdKeys.Add(demo.Key);
                }

                agentIds[demo.Key] = agent.Id;
            }

            List<Property> properties = SeedData.GetProperties()
                .Where(p => createdKeys.Contains(p.AgentId))
                .ToList();

            foreach (Property property in properties)
            {
                property.AgentId = agentIds[property.AgentId];
                property.Id = 0;
            }

            if (properties.Count > 0)
            {
                context.Properties.AddRange(properties);
                await context.SaveChangesAsync();
            }
        }
    }
}
