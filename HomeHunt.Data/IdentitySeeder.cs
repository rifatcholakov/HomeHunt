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

            await EnsureUserExistsAsync(
                userManager,
                configuration[ConfigKeys.AdminEmail]!,
                configuration[ConfigKeys.AdminPassword]!,
                Roles.Administrator,
                Messages.Account.AdminCreateFailedMessage);

            await EnsureUserExistsAsync(
                userManager,
                configuration[ConfigKeys.DemoUserEmail]!,
                configuration[ConfigKeys.DemoUserPassword]!,
                Roles.User,
                Messages.Account.DemoAccountCreateFailedMessage);

            await SeedDemoAgentsWithPropertiesAsync(userManager, context, configuration[ConfigKeys.DemoAgentPassword]!);

            await EnsureEachUserHasOneRoleAsync(userManager, context);
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
            string role,
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

            await userManager.SetSingleRoleAsync(user, role);

            return user;
        }

        private static async Task EnsureEachUserHasOneRoleAsync(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            HashSet<string> agentUserIds = (await context.Agents.Select(a => a.UserId).ToListAsync()).ToHashSet();

            var userRoles = await (
                from userRole in context.UserRoles
                join role in context.Roles on userRole.RoleId equals role.Id
                select new { userRole.UserId, RoleName = role.Name! })
                .ToListAsync();

            Dictionary<string, List<string>> rolesByUser = userRoles
                .GroupBy(r => r.UserId)
                .ToDictionary(g => g.Key, g => g.Select(r => r.RoleName).ToList());

            foreach (IdentityUser user in await userManager.Users.ToListAsync())
            {
                List<string> currentRoles = rolesByUser.GetValueOrDefault(user.Id) ?? new List<string>();

                string expectedRole = currentRoles.Contains(Roles.Administrator) ? Roles.Administrator
                    : currentRoles.Contains(Roles.Agent) ? Roles.Agent
                    : currentRoles.Contains(Roles.User) ? Roles.User
                    : agentUserIds.Contains(user.Id) ? Roles.Agent : Roles.User;

                if (currentRoles.Count != 1 || currentRoles[0] != expectedRole)
                {
                    await userManager.SetSingleRoleAsync(user, expectedRole);
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
                    Roles.Agent,
                    Messages.Account.DemoAccountCreateFailedMessage);

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
