using HomeHunt.Common.Enums;
using HomeHunt.Data;
using HomeHunt.Service.Data.Dashboard;
using HomeHunt.Service.Data.Dashboard.Response;
using Microsoft.EntityFrameworkCore;

namespace HomeHunt.Services
{
    public class DashboardService : IDashboardService
    {
        private const int RecentPropertiesCount = 5;
        private const int TopAgentsCount = 5;

        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetDashboardResponseData> GetDashboardAsync()
        {
            return new GetDashboardResponseData
            {
                PropertyCount = await _context.Properties.CountAsync(),
                AvailablePropertyCount = await _context.Properties.CountAsync(p => p.Status == PropertyStatus.Available),
                AgentCount = await _context.Agents.CountAsync(),
                LocationCount = await _context.Locations.CountAsync(),
                UserCount = await _context.Users.CountAsync(),
                FavoriteCount = await _context.Favorites.CountAsync(),
                ForSaleCount = await _context.Properties.CountAsync(p => p.ListingType == ListingType.Sale),
                ForRentCount = await _context.Properties.CountAsync(p => p.ListingType == ListingType.Rent),
                StatusCounts = await _context.Properties
                    .GroupBy(p => p.Status)
                    .Select(g => new GetStatusCountResponseData
                    {
                        Status = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync(),
                RecentProperties = await _context.Properties
                    .AsNoTracking()
                    .OrderByDescending(p => p.CreatedOn)
                    .Take(RecentPropertiesCount)
                    .Select(p => new GetDashboardPropertyResponseData
                    {
                        Id = p.Id,
                        Title = p.Title,
                        City = p.Location.City,
                        Price = p.Price,
                        ListingType = p.ListingType,
                        Status = p.Status,
                        CreatedOn = p.CreatedOn
                    })
                    .ToListAsync(),
                TopAgents = await _context.Agents
                    .AsNoTracking()
                    .OrderByDescending(a => a.Properties.Count)
                    .ThenBy(a => a.FirstName)
                    .Take(TopAgentsCount)
                    .Select(a => new GetDashboardAgentResponseData
                    {
                        Id = a.Id,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        PropertyCount = a.Properties.Count
                    })
                    .ToListAsync()
            };
        }
    }
}
