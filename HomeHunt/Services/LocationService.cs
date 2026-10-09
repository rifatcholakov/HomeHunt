using HomeHunt.Data;
using HomeHunt.Data.Models;
using HomeHunt.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HomeHunt.Services
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;

        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LocationListViewModel>> GetAllAsync()
        {
            return await _context.Locations
                .AsNoTracking()
                .OrderBy(l => l.City)
                .ThenBy(l => l.Neighborhood)
                .Select(l => new LocationListViewModel
                {
                    Id = l.Id,
                    City = l.City,
                    Neighborhood = l.Neighborhood,
                    PostalCode = l.PostalCode,
                    PropertyCount = l.Properties.Count
                })
                .ToListAsync();
        }

        public async Task CreateAsync(LocationFormViewModel model)
        {
            Location location = new Location
            {
                City = model.City,
                Neighborhood = model.Neighborhood,
                PostalCode = model.PostalCode
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
        }
    }
}
