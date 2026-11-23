using HomeHunt.Common;
using HomeHunt.Data;
using HomeHunt.Data.Models;
using HomeHunt.Common.Enums;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;

        public AgentService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                    Email = a.User.Email!,
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
                    Email = a.User.Email!,
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
                            ImageUrl = p.ImageUrl,
                            OwnerUserId = p.Agent.UserId
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
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agent == null)
            {
                return false;
            }

            IdentityUser user = agent.User;

            _context.Properties.RemoveRange(agent.Properties);
            _context.Agents.Remove(agent);
            await _context.SaveChangesAsync();

            await _userManager.DeleteAsync(user);
            return true;
        }

        public async Task<bool> AgentExistsAsync(int id)
        {
            return await _context.Agents
                .AnyAsync(a => a.Id == id);
        }

        public async Task<int?> GetAgentIdByUserIdAsync(string userId)
        {
            return await _context.Agents
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .Select(a => (int?)a.Id)
                .FirstOrDefaultAsync();
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

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
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
                    Email = a.User.Email!,
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

            agent.FirstName = Helpers.CapitalizeFirstLetter(request.FirstName);
            agent.LastName = Helpers.CapitalizeFirstLetter(request.LastName);
            agent.Phone = request.Phone;
            agent.AgencyName = request.AgencyName;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CreateAgentResponseData> CreateAgentAsync(CreateAgentRequestData request)
        {
            CreateAgentResponseData response = new CreateAgentResponseData();

            string email = request.Email.ToLowerInvariant();

            IdentityUser user = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            IdentityResult createResult = await _userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                response.Errors.AddRange(createResult.Errors.Select(e => e.Description));
                return response;
            }

            IdentityResult roleResult = await _userManager.AddToRolesAsync(user, new[] { Roles.User, Roles.Agent });

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                response.Errors.AddRange(roleResult.Errors.Select(e => e.Description));
                return response;
            }

            Agent agent = new Agent
            {
                FirstName = Helpers.CapitalizeFirstLetter(request.FirstName),
                LastName = Helpers.CapitalizeFirstLetter(request.LastName),
                Phone = request.Phone,
                AgencyName = request.AgencyName,
                UserId = user.Id
            };

            try
            {
                _context.Agents.Add(agent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                await _userManager.DeleteAsync(user);
                response.Errors.Add(Messages.Agent.SaveFailedMessage);
                return response;
            }

            response.Succeeded = true;
            return response;
        }

        public async Task<CreateAgentResponseData> UpgradeToAgentAsync(UpgradeToAgentRequestData request)
        {
            CreateAgentResponseData response = new CreateAgentResponseData();

            IdentityUser? user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                response.Errors.Add(Messages.Agent.SaveFailedMessage);
                return response;
            }

            bool alreadyAgent = await _context.Agents.AnyAsync(a => a.UserId == user.Id);

            if (alreadyAgent)
            {
                response.Errors.Add(Messages.Agent.AlreadyAgentMessage);
                return response;
            }

            IdentityResult roleResult = await _userManager.AddToRoleAsync(user, Roles.Agent);

            if (!roleResult.Succeeded)
            {
                response.Errors.AddRange(roleResult.Errors.Select(e => e.Description));
                return response;
            }

            Agent agent = new Agent
            {
                FirstName = Helpers.CapitalizeFirstLetter(request.FirstName),
                LastName = Helpers.CapitalizeFirstLetter(request.LastName),
                Phone = request.Phone,
                AgencyName = request.AgencyName,
                UserId = user.Id
            };

            try
            {
                _context.Agents.Add(agent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                await _userManager.RemoveFromRoleAsync(user, Roles.Agent);
                response.Errors.Add(Messages.Agent.SaveFailedMessage);
                return response;
            }

            response.Succeeded = true;
            return response;
        }
    }
}
