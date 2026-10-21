using HomeHunt.Data;
using HomeHunt.Data.Models;
using HomeHunt.Common.Enums;
using Microsoft.EntityFrameworkCore;
using HomeHunt.Service.Data.Properties;
using HomeHunt.Service.Data.Properties.Request;
using HomeHunt.Service.Data.Properties.Response;

namespace HomeHunt.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;

        public PropertyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetPropertiesResponseData>> SearchPropertiesAsync(SearchPropertiesRequestData request)
        {
            IQueryable<Property> query = _context.Properties.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.City))
            {
                query = query.Where(p => p.Location.City == request.City);
            }

            if (request.PropertyType.HasValue)
            {
                query = query.Where(p => p.PropertyType == request.PropertyType.Value);
            }

            if (request.ListingType.HasValue)
            {
                query = query.Where(p => p.ListingType == request.ListingType.Value);
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            if (request.MinBedrooms.HasValue)
            {
                query = query.Where(p => p.Bedrooms >= request.MinBedrooms.Value);
            }

            if (request.MinArea.HasValue)
            {
                query = query.Where(p => p.Area >= request.MinArea.Value);
            }

            if (request.SortBy == "priceAsc")
            {
                query = query.OrderBy(p => p.Price);
            }
            else if (request.SortBy == "priceDesc")
            {
                query = query.OrderByDescending(p => p.Price);
            }
            else if (request.SortBy == "area")
            {
                query = query.OrderByDescending(p => p.Area);
            }
            else
            {
                query = query.OrderByDescending(p => p.CreatedOn);
            }

            return await query
                .Select(p => new GetPropertiesResponseData
                {
                    Id = p.Id,
                    Title = p.Title,
                    City = p.Location.City,
                    Neighborhood = p.Location.Neighborhood,
                    Price = p.Price,
                    Area = p.Area,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    PropertyType = p.PropertyType,
                    ListingType = p.ListingType,
                    Status = p.Status,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<GetPropertiesResponseData>> GetRecentPropertiesAsync(int count)
        {
            return await _context.Properties
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedOn)
                .Take(count)
                .Select(p => new GetPropertiesResponseData
                {
                    Id = p.Id,
                    Title = p.Title,
                    City = p.Location.City,
                    Neighborhood = p.Location.Neighborhood,
                    Price = p.Price,
                    Area = p.Area,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    PropertyType = p.PropertyType,
                    ListingType = p.ListingType,
                    Status = p.Status,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();
        }

        public async Task<int> CountAvailablePropertiesAsync()
        {
            return await _context.Properties
                .CountAsync(p => p.Status == PropertyStatus.Available);
        }

        public async Task<int> CountAvailablePropertiesForSaleAsync()
        {
            return await _context.Properties
                .CountAsync(p => p.Status == PropertyStatus.Available && p.ListingType == ListingType.Sale);
        }

        public async Task<int> CountAvailablePropertiesForRentAsync()
        {
            return await _context.Properties
                .CountAsync(p => p.Status == PropertyStatus.Available && p.ListingType == ListingType.Rent);
        }

        public async Task CreatePropertyAsync(CreatePropertyRequestData request)
        {
            Property property = new Property
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Area = request.Area,
                Bedrooms = request.Bedrooms,
                Bathrooms = request.Bathrooms,
                PropertyType = request.PropertyType,
                ListingType = request.ListingType,
                Status = request.Status,
                ImageUrl = request.ImageUrl,
                LocationId = request.LocationId,
                AgentId = request.AgentId,
                CreatedOn = DateTime.Now
            };

            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
        }

        public async Task<GetPropertyForEditResponseData?> GetPropertyForEditAsync(int id)
        {
            return await _context.Properties
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new GetPropertyForEditResponseData
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    Area = p.Area,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    PropertyType = p.PropertyType,
                    ListingType = p.ListingType,
                    Status = p.Status,
                    ImageUrl = p.ImageUrl,
                    LocationId = p.LocationId,
                    AgentId = p.AgentId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatePropertyAsync(int id, UpdatePropertyRequestData request)
        {
            Property? property = await _context.Properties.FindAsync(id);

            if (property == null)
            {
                return false;
            }

            property.Title = request.Title;
            property.Description = request.Description;
            property.Price = request.Price;
            property.Area = request.Area;
            property.Bedrooms = request.Bedrooms;
            property.Bathrooms = request.Bathrooms;
            property.PropertyType = request.PropertyType;
            property.ListingType = request.ListingType;
            property.Status = request.Status;
            property.ImageUrl = request.ImageUrl;
            property.LocationId = request.LocationId;
            property.AgentId = request.AgentId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePropertyAsync(int id)
        {
            Property? property = await _context.Properties.FindAsync(id);

            if (property == null)
            {
                return false;
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<GetPropertyDetailsResponseData?> GetPropertyDetailsAsync(int id)
        {
            return await _context.Properties
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new GetPropertyDetailsResponseData
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    Area = p.Area,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    PropertyType = p.PropertyType,
                    ListingType = p.ListingType,
                    Status = p.Status,
                    ImageUrl = p.ImageUrl,
                    CreatedOn = p.CreatedOn,
                    LocationId = p.LocationId,
                    City = p.Location.City,
                    Neighborhood = p.Location.Neighborhood,
                    AgentId = p.AgentId,
                    AgentFirstName = p.Agent.FirstName,
                    AgentLastName = p.Agent.LastName,
                    AgentEmail = p.Agent.Email,
                    AgentPhone = p.Agent.Phone
                })
                .FirstOrDefaultAsync();
        }
    }
}
