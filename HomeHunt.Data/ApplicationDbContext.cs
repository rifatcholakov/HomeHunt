using HomeHunt.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeHunt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Property> Properties { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Agent> Agents { get; set; } = null!;
        public DbSet<FavoriteProperty> Favorites { get; set; } = null!;
        public DbSet<PropertyImage> PropertyImages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Location>()
                .HasIndex(l => new { l.City, l.Neighborhood })
                .IsUnique();

            builder.Entity<Agent>()
                .HasIndex(a => a.UserId)
                .IsUnique();

            builder.Entity<Agent>()
                .HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<Agent>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Agent>()
                .HasIndex(a => a.Phone)
                .IsUnique();

            builder.Entity<Property>()
                .HasOne(p => p.Location)
                .WithMany(l => l.Properties)
                .HasForeignKey(p => p.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Property>()
                .HasOne(p => p.Agent)
                .WithMany(a => a.Properties)
                .HasForeignKey(p => p.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PropertyImage>()
                .HasOne(i => i.Property)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FavoriteProperty>()
                .HasKey(f => new { f.UserId, f.PropertyId });

            builder.Entity<FavoriteProperty>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FavoriteProperty>()
                .HasOne(f => f.Property)
                .WithMany()
                .HasForeignKey(f => f.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Location>().HasData(SeedData.GetLocations());
        }
    }
}
