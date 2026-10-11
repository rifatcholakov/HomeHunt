using HomeHunt.Data;
using HomeHunt.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HomeHunt.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;

        public PropertyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PropertyListViewModel>> GetAllAsync()
        {
            return await _context.Properties
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedOn)
                .Select(p => new PropertyListViewModel
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

        public async Task<PropertyDetailsViewModel?> GetByIdAsync(int id)
        {
            return await _context.Properties
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new PropertyDetailsViewModel
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
