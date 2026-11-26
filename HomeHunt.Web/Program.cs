using HomeHunt.Authorization;
using HomeHunt.Common;
using HomeHunt.Data;
using HomeHunt.Service.Data.Agents;
using HomeHunt.Service.Data.Dashboard;
using HomeHunt.Service.Data.Favorites;
using HomeHunt.Service.Data.Locations;
using HomeHunt.Service.Data.Properties;
using HomeHunt.Service.Data.Users;
using HomeHunt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeHunt
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.CanManageProperties, policy => policy.RequireRole(Roles.Administrator, Roles.Agent));
                options.AddPolicy(Policies.CanModifyProperty, policy => policy.AddRequirements(new PropertyOwnerRequirement()));
                options.AddPolicy(Policies.CanManageLocations, policy => policy.RequireRole(Roles.Administrator));
                options.AddPolicy(Policies.CanManageAgents, policy => policy.RequireRole(Roles.Administrator));
                options.AddPolicy(Policies.CanAccessAdmin, policy => policy.RequireRole(Roles.Administrator));
            });

            builder.Services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.Zero);

            builder.Services.AddSingleton<IAuthorizationHandler, PropertyOwnerHandler>();

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            builder.Services.AddScoped<IPropertyService, PropertyService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IAgentService, AgentService>();
            builder.Services.AddScoped<IFavoriteService, FavoriteService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                await IdentitySeeder.SeedAsync(scope.ServiceProvider);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
