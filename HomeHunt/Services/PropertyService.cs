using HomeHunt.Data;
using HomeHunt.Data.Models;
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

        public async Task CreateAsync(PropertyFormViewModel model)
        {
            Property property = new Property
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price ?? 0,
                Area = model.Area ?? 0,
                Bedrooms = model.Bedrooms,
                Bathrooms = model.Bathrooms,
                PropertyType = model.PropertyType,
                ListingType = model.ListingType,
                Status = model.Status,
                ImageUrl = model.ImageUrl,
                LocationId = model.LocationId ?? 0,
                AgentId = model.AgentId ?? 0,
                CreatedOn = DateTime.Now
            };

            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
        }

        public async Task<PropertyFormViewModel?> GetEditModelAsync(int id)
        {
            return await _context.Properties
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new PropertyFormViewModel
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

        public async Task<bool> UpdateAsync(int id, PropertyFormViewModel model)
        {
            Property? property = await _context.Properties.FindAsync(id);

            if (property == null)
            {
                return false;
            }

            property.Title = model.Title;
            property.Description = model.Description;
            property.Price = model.Price ?? 0;
            property.Area = model.Area ?? 0;
            property.Bedrooms = model.Bedrooms;
            property.Bathrooms = model.Bathrooms;
            property.PropertyType = model.PropertyType;
            property.ListingType = model.ListingType;
            property.Status = model.Status;
            property.ImageUrl = model.ImageUrl;
            property.LocationId = model.LocationId ?? 0;
            property.AgentId = model.AgentId ?? 0;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
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
