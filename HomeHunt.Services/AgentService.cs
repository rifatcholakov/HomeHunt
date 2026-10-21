using HomeHunt.Common;
using HomeHunt.Data;
using HomeHunt.Data.Models;
using HomeHunt.Common.Enums;
using Microsoft.EntityFrameworkCore;
using HomeHunt.Service.Data.Agents;
using HomeHunt.Service.Data.Agents.Request;
using HomeHunt.Service.Data.Agents.Response;
using HomeHunt.Service.Data.Properties.Response;

namespace HomeHunt.Services
{
    public class AgentService : IAgentService
    {
        private readonly ApplicationDbContext _context;

        public AgentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetAllAgentsResponseData>> GetAllAgentsAsync()
        {
            return await _context.Agents
                .AsNoTracking()
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => new GetAllAgentsResponseData
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

        public async Task<GetAgentDetailsResponseData?> GetAgentDetailsAsync(int id)
        {
            GetAgentDetailsResponseData? model = await _context.Agents
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new GetAgentDetailsResponseData
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    AgencyName = a.AgencyName,
                    Email = a.Email,
                    Phone = a.Phone,
                    Properties = a.Properties
                        .OrderByDescending(p => p.CreatedOn)
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

        public async Task<bool> DeleteAgentAsync(int id)
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

        public async Task<bool> AgentExistsAsync(int id)
        {
            return await _context.Agents
                .AnyAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<GetAgentOptionsResponseData>> GetAgentOptionsAsync()
        {
            return await _context.Agents
                .AsNoTracking()
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => new GetAgentOptionsResponseData
                {
                    Id = a.Id,
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

        public async Task<GetAgentForEditResponseData?> GetAgentForEditAsync(int id)
        {
            return await _context.Agents
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new GetAgentForEditResponseData
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

        public async Task<bool> UpdateAgentAsync(int id, UpdateAgentRequestData request)
        {
            Agent? agent = await _context.Agents.FindAsync(id);

            if (agent == null)
            {
                return false;
            }

            agent.FirstName = Helpers.Capitalize(request.FirstName);
            agent.LastName = Helpers.Capitalize(request.LastName);
            agent.Email = request.Email.ToLowerInvariant();
            agent.Phone = request.Phone;
            agent.AgencyName = request.AgencyName;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task CreateAgentAsync(CreateAgentRequestData request)
        {
            Agent agent = new Agent
            {
                FirstName = Helpers.Capitalize(request.FirstName),
                LastName = Helpers.Capitalize(request.LastName),
                Email = request.Email.ToLowerInvariant(),
                Phone = request.Phone,
                AgencyName = request.AgencyName
            };

            _context.Agents.Add(agent);
            await _context.SaveChangesAsync();
        }
    }
}
