using HomeHunt.Common;
using HomeHunt.Data;
using HomeHunt.Data.Models;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<LocationDetailsViewModel?> GetByIdAsync(int id)
        {
            return await _context.Locations
                .AsNoTracking()
                .Where(l => l.Id == id)
                .Select(l => new LocationDetailsViewModel
                {
                    Id = l.Id,
                    City = l.City,
                    Neighborhood = l.Neighborhood,
                    PostalCode = l.PostalCode,
                    PropertyCount = l.Properties.Count
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<CityCountViewModel>> GetCityCountsAsync()
        {
            return await _context.Properties
                .AsNoTracking()
                .GroupBy(p => p.Location.City)
                .Select(g => new CityCountViewModel
                {
                    City = g.Key,
                    PropertyCount = g.Count()
                })
                .OrderByDescending(c => c.PropertyCount)
                .ThenBy(c => c.City)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Locations
                .AnyAsync(l => l.Id == id);
        }

        public async Task<List<SelectListItem>> GetOptionsAsync()
        {
            return await _context.Locations
                .AsNoTracking()
                .OrderBy(l => l.City)
                .ThenBy(l => l.Neighborhood)
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.City + ", " + l.Neighborhood
                })
                .ToListAsync();
        }

        public async Task<List<string>> GetCitiesAsync()
        {
            return await _context.Locations
                .AsNoTracking()
                .Select(l => l.City)
                .Distinct()
                .OrderBy(city => city)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string city, string neighborhood)
        {
            return await _context.Locations
                .AnyAsync(l => l.City == city && l.Neighborhood == neighborhood);
        }

        public async Task<string?> GetCityByPostalCodeAsync(string postalCode)
        {
            return await _context.Locations
                .AsNoTracking()
                .Where(l => l.PostalCode == postalCode)
                .Select(l => l.City)
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(LocationFormViewModel model)
        {
            Location location = new Location
            {
                City = Helpers.Capitalize(model.City),
                Neighborhood = Helpers.Capitalize(model.Neighborhood),
                PostalCode = model.PostalCode
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
        }
    }
}
