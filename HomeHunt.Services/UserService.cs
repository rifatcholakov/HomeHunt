using HomeHunt.Common;
using HomeHunt.Data;
using HomeHunt.Data.Models;
using HomeHunt.Service.Data;
using HomeHunt.Service.Data.Agents;
using HomeHunt.Service.Data.Agents.Request;
using HomeHunt.Service.Data.Agents.Response;
using HomeHunt.Service.Data.Users;
using HomeHunt.Service.Data.Users.Request;
using HomeHunt.Service.Data.Users.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HomeHunt.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAgentService _agentService;

        public UserService(ApplicationDbContext context, UserManager<IdentityUser> userManager, IAgentService agentService)
        {
            _context = context;
            _userManager = userManager;
            _agentService = agentService;
        }

        public async Task<PagedResponseData<GetUserResponseData>> GetUsersAsync(string? search, int page, int pageSize)
        {
            IQueryable<GetUserResponseData> query = BuildUsersQuery();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string term = search.Trim();
                query = query.Where(u => u.Email.Contains(term));
            }

            PagedResponseData<GetUserResponseData> response = await query
                .OrderBy(u => u.Email)
                .ToPagedAsync(page, pageSize);

            await LoadRolesAsync(response.Items);

            return response;
        }

        public async Task<GetUserResponseData?> GetUserAsync(string id)
        {
            GetUserResponseData? user = await BuildUsersQuery().FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
                await LoadRolesAsync(new List<GetUserResponseData> { user });
            }

            return user;
        }

        public async Task<UserOperationResponseData> ChangeRoleAsync(ChangeUserRoleRequestData request)
        {
            UserOperationResponseData response = new UserOperationResponseData();

            if (!Roles.All.Contains(request.NewRole))
            {
                response.Errors.Add(Messages.Admin.RoleInvalidMessage);
                return response;
            }

            IdentityUser? user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                response.Errors.Add(Messages.Admin.UserNotFoundMessage);
                return response;
            }

            IList<string> currentRoles = await _userManager.GetRolesAsync(user);
            string? currentRole = currentRoles.FirstOrDefault();

            if (currentRoles.Count == 1 && currentRole == request.NewRole)
            {
                response.Succeeded = true;
                return response;
            }

            Agent? agent = await _context.Agents.FirstOrDefaultAsync(a => a.UserId == user.Id);

            await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

            List<string> errors = request.NewRole == Roles.Agent && agent == null
                ? await PromoteToAgentAsync(user, request)
                : await ApplyRoleAsync(user, agent, request.NewRole);

            if (errors.Count > 0)
            {
                response.Errors.AddRange(errors);
                return response;
            }

            await _userManager.UpdateSecurityStampAsync(user);
            await transaction.CommitAsync();

            response.Succeeded = true;
            return response;
        }

        public async Task<UserOperationResponseData> DeleteUserAsync(string id)
        {
            UserOperationResponseData response = new UserOperationResponseData();

            IdentityUser? user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                response.Errors.Add(Messages.Admin.UserNotFoundMessage);
                return response;
            }

            if (await _userManager.IsInRoleAsync(user, Roles.Administrator))
            {
                response.Errors.Add(Messages.Admin.CannotDeleteAdministratorMessage);
                return response;
            }

            int? agentId = await _agentService.GetAgentIdByUserIdAsync(id);

            if (agentId != null)
            {
                await _agentService.DeleteAgentAsync(agentId.Value);
                response.Succeeded = true;
                return response;
            }

            IdentityResult result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                response.Errors.AddRange(result.Errors.Select(e => e.Description));
                return response;
            }

            response.Succeeded = true;
            return response;
        }

        private async Task<List<string>> PromoteToAgentAsync(IdentityUser user, ChangeUserRoleRequestData request)
        {
            CreateAgentResponseData result = await _agentService.UpgradeToAgentAsync(new UpgradeToAgentRequestData
            {
                UserId = user.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                AgencyName = request.AgencyName
            });

            return result.Errors.ToList();
        }

        private async Task<List<string>> ApplyRoleAsync(IdentityUser user, Agent? agent, string newRole)
        {
            if (newRole == Roles.User && agent != null)
            {
                bool hasProperties = await _context.Properties.AnyAsync(p => p.AgentId == agent.Id);

                if (hasProperties)
                {
                    return new List<string> { Messages.Admin.AgentHasPropertiesMessage };
                }

                _context.Agents.Remove(agent);
                await _context.SaveChangesAsync();
            }

            IdentityResult result = await _userManager.SetSingleRoleAsync(user, newRole);

            return result.Errors.Select(e => e.Description).ToList();
        }

        private IQueryable<GetUserResponseData> BuildUsersQuery()
        {
            return _context.Users
                .AsNoTracking()
                .Select(u => new GetUserResponseData
                {
                    Id = u.Id,
                    Email = u.Email!,
                    AgentId = _context.Agents
                        .Where(a => a.UserId == u.Id)
                        .Select(a => (int?)a.Id)
                        .FirstOrDefault(),
                    PropertyCount = _context.Properties.Count(p => p.Agent.UserId == u.Id)
                });
        }

        private async Task LoadRolesAsync(List<GetUserResponseData> users)
        {
            List<string> userIds = users.Select(u => u.Id).ToList();

            var userRoles = await (
                from userRole in _context.UserRoles
                join role in _context.Roles on userRole.RoleId equals role.Id
                where userIds.Contains(userRole.UserId)
                select new { userRole.UserId, RoleName = role.Name! })
                .ToListAsync();

            foreach (GetUserResponseData user in users)
            {
                user.Roles = userRoles
                    .Where(r => r.UserId == user.Id)
                    .Select(r => r.RoleName)
                    .OrderBy(name => name)
                    .ToList();
            }
        }
    }
}
