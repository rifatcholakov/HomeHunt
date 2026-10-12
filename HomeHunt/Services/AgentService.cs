using HomeHunt.Common;
using HomeHunt.Data;
using HomeHunt.Data.Models;
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
            return await _context.Agents
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
                    PropertyCount = a.Properties.Count
                })
                .FirstOrDefaultAsync();
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

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Agents
                .AnyAsync(a => a.Email == email);
        }

        public async Task<bool> PhoneExistsAsync(string phone)
        {
            return await _context.Agents
                .AnyAsync(a => a.Phone == phone);
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
