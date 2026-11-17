using HomeHunt.Common;
using HomeHunt.Data;
using HomeHunt.Service.Data.Agents;
using HomeHunt.Service.Data.Locations;
using HomeHunt.Service.Data.Properties;
using HomeHunt.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
                options.AddPolicy(Policies.CanManageProperties, policy =>
                    policy.RequireRole(Roles.Administrator, Roles.Agent));

                options.AddPolicy(Policies.CanManageLocations, policy =>
                    policy.RequireRole(Roles.Administrator));

                options.AddPolicy(Policies.CanManageAgents, policy =>
                    policy.RequireRole(Roles.Administrator));
            });
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            builder.Services.AddScoped<IPropertyService, PropertyService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IAgentService, AgentService>();

            var app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                string adminEmail = builder.Configuration["AdminAccount:Email"]!;
                string adminPassword = builder.Configuration["AdminAccount:Password"]!;
                
                await IdentitySeeder.SeedAsync(scope.ServiceProvider, adminEmail, adminPassword);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
