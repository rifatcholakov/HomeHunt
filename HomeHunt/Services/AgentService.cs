using HomeHunt.Common;
using HomeHunt.Data;
using HomeHunt.Data.Models;
using HomeHunt.Data.Models.Enums;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HomeHunt.Services
{
    public class AgentService : IAgentService
    {
        private readonly ApplicationDbContext _context;

        public AgentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AgentListViewModel>> GetAllAsync()
        {
            return await _context.Agents
                .AsNoTracking()
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => new AgentListViewModel
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    AgencyName = a.AgencyName,
                    Email = a.Email,
                    Phone = a.Phone,
                    PropertyCount = a.Properties.Count
                })
                .ToListAsync();
        }

        public async Task<AgentDetailsViewModel?> GetByIdAsync(int id)
        {
            AgentDetailsViewModel? model = await _context.Agents
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AgentDetailsViewModel
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    AgencyName = a.AgencyName,
                    Email = a.Email,
                    Phone = a.Phone,
                    Properties = a.Properties
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

        public async Task<bool> DeleteAsync(int id)
        {
            Agent? agent = await _context.Agents
                .Include(a => a.Properties)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agent == null)
            {
                return false;
            }

            _context.Properties.RemoveRange(agent.Properties);
            _context.Agents.Remove(agent);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Agents
                .AnyAsync(a => a.Id == id);
        }

        public async Task<List<SelectListItem>> GetOptionsAsync()
        {
            return await _context.Agents
                .AsNoTracking()
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.FirstName + " " + a.LastName
                })
                .ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email, int excludeId = 0)
        {
            return await _context.Agents
                .AnyAsync(a => a.Email == email && a.Id != excludeId);
        }

        public async Task<bool> PhoneExistsAsync(string phone, int excludeId = 0)
        {
            return await _context.Agents
                .AnyAsync(a => a.Phone == phone && a.Id != excludeId);
        }

        public async Task<AgentFormViewModel?> GetEditModelAsync(int id)
        {
            return await _context.Agents
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AgentFormViewModel
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Email = a.Email,
                    Phone = a.Phone,
                    AgencyName = a.AgencyName
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(int id, AgentFormViewModel model)
        {
            Agent? agent = await _context.Agents.FindAsync(id);

            if (agent == null)
            {
                return false;
            }

            agent.FirstName = Helpers.Capitalize(model.FirstName);
            agent.LastName = Helpers.Capitalize(model.LastName);
            agent.Email = model.Email.ToLowerInvariant();
            agent.Phone = model.Phone;
            agent.AgencyName = model.AgencyName;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task CreateAsync(AgentFormViewModel model)
        {
            Agent agent = new Agent
            {
                FirstName = Helpers.Capitalize(model.FirstName),
                LastName = Helpers.Capitalize(model.LastName),
                Email = model.Email.ToLowerInvariant(),
                Phone = model.Phone,
                AgencyName = model.AgencyName
            };

            _context.Agents.Add(agent);
            await _context.SaveChangesAsync();
        }
    }
}
