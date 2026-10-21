using HomeHunt.Common;
using HomeHunt.Data;
using HomeHunt.Data.Models;
using HomeHunt.Common.Enums;
using Microsoft.EntityFrameworkCore;
using HomeHunt.Service.Data.Locations;
using HomeHunt.Service.Data.Locations.Request;
using HomeHunt.Service.Data.Locations.Response;
using HomeHunt.Service.Data.Properties.Response;

namespace HomeHunt.Services
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;

        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetAllLocationsResponseData>> GetAllLocationsAsync()
        {
            return await _context.Locations
                .AsNoTracking()
                .OrderBy(l => l.City)
                .ThenBy(l => l.Neighborhood)
                .Select(l => new GetAllLocationsResponseData
                {
                    Id = l.Id,
                    City = l.City,
                    Neighborhood = l.Neighborhood,
                    PostalCode = l.PostalCode,
                    PropertyCount = l.Properties.Count
                })
                .ToListAsync();
        }

        public async Task<GetLocationDetailsResponseData?> GetLocationDetailsAsync(int id)
        {
            GetLocationDetailsResponseData? model = await _context.Locations
                .AsNoTracking()
                .Where(l => l.Id == id)
                .Select(l => new GetLocationDetailsResponseData
                {
                    Id = l.Id,
                    City = l.City,
                    Neighborhood = l.Neighborhood,
                    PostalCode = l.PostalCode,
                    Properties = l.Properties
                        .OrderByDescending(p => p.CreatedOn)
                        .Select(p => new GetPropertiesResponseData
                        {
                            Id = p.Id,
                            Title = p.Title,
                            City = l.City,
                            Neighborhood = l.Neighborhood,
                            Price = p.Price,
                            Area = p.Area,
                            Bedrooms = p.Bedrooms,
                            Bathrooms = p.Bathrooms,
                            PropertyType = p.PropertyType,
                            ListingType = p.ListingType,
                            Status = p.Status,
                            ImageUrl = p.ImageUrl
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return null;
            }

            model.PropertyCount = model.Properties.Count;
            model.ForSaleCount = model.Properties.Count(p => p.Status == PropertyStatus.Available && p.ListingType == ListingType.Sale);
            model.ForRentCount = model.Properties.Count(p => p.Status == PropertyStatus.Available && p.ListingType == ListingType.Rent);

            return model;
        }

        public async Task<IEnumerable<GetCityCountsResponseData>> GetCityCountsAsync()
        {
            return await _context.Properties
                .AsNoTracking()
                .GroupBy(p => p.Location.City)
                .Select(g => new GetCityCountsResponseData
                {
                    City = g.Key,
                    PropertyCount = g.Count()
                })
                .OrderByDescending(c => c.PropertyCount)
                .ThenBy(c => c.City)
                .ToListAsync();
        }

        public async Task<bool> LocationExistsAsync(int id)
        {
            return await _context.Locations
                .AnyAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<GetLocationOptionsResponseData>> GetLocationOptionsAsync()
        {
            return await _context.Locations
                .AsNoTracking()
                .OrderBy(l => l.City)
                .ThenBy(l => l.Neighborhood)
                .Select(l => new GetLocationOptionsResponseData
                {
                    Id = l.Id,
                    Text = l.City + ", " + l.Neighborhood
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetCitiesAsync()
        {
            return await _context.Locations
                .AsNoTracking()
                .Select(l => l.City)
                .Distinct()
                .OrderBy(city => city)
                .ToListAsync();
        }

        public async Task<bool> LocationExistsAsync(string city, string neighborhood, int excludeId = 0)
        {
            return await _context.Locations
                .AnyAsync(l => l.City == city && l.Neighborhood == neighborhood && l.Id != excludeId);
        }

        public async Task<string?> GetCityByPostalCodeAsync(string postalCode, int excludeId = 0)
        {
            return await _context.Locations
                .AsNoTracking()
                .Where(l => l.PostalCode == postalCode && l.Id != excludeId)
                .Select(l => l.City)
                .FirstOrDefaultAsync();
        }

        public async Task<GetLocationForEditResponseData?> GetLocationForEditAsync(int id)
        {
            return await _context.Locations
                .AsNoTracking()
                .Where(l => l.Id == id)
                .Select(l => new GetLocationForEditResponseData
                {
                    Id = l.Id,
                    City = l.City,
                    Neighborhood = l.Neighborhood,
                    PostalCode = l.PostalCode
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteLocationAsync(int id)
        {
            Location? location = await _context.Locations
                .Include(l => l.Properties)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (location == null)
            {
                return false;
            }

            _context.Properties.RemoveRange(location.Properties);
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateLocationAsync(int id, UpdateLocationRequestData request)
        {
            Location? location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                return false;
            }

            location.City = Helpers.Capitalize(request.City);
            location.Neighborhood = Helpers.Capitalize(request.Neighborhood);
            location.PostalCode = request.PostalCode;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task CreateLocationAsync(CreateLocationRequestData request)
        {
            Location location = new Location
            {
                City = Helpers.Capitalize(request.City),
                Neighborhood = Helpers.Capitalize(request.Neighborhood),
                PostalCode = request.PostalCode
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
        }
    }
}
