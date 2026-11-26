using HomeHunt.Common.Enums;
using HomeHunt.Service.Data.Dashboard;
using HomeHunt.Service.Data.Dashboard.Response;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomeHunt.Areas.Admin.Controllers
{
    public class DashboardController : AdminControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            GetDashboardResponseData response = await _dashboardService.GetDashboardAsync();

            DashboardViewModel model = new DashboardViewModel
            {
                PropertyCount = response.PropertyCount,
                AvailablePropertyCount = response.AvailablePropertyCount,
                AgentCount = response.AgentCount,
                LocationCount = response.LocationCount,
                UserCount = response.UserCount,
                FavoriteCount = response.FavoriteCount,
                ForSaleCount = response.ForSaleCount,
                ForRentCount = response.ForRentCount,
                StatusCounts = Enum.GetValues<PropertyStatus>()
                    .Select(status => new StatusCountViewModel
                    {
                        Status = status,
                        Count = response.StatusCounts.FirstOrDefault(s => s.Status == status)?.Count ?? 0
                    })
                    .ToList(),
                RecentProperties = response.RecentProperties
                    .Select(p => new DashboardPropertyViewModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        City = p.City,
                        Price = p.Price,
                        ListingType = p.ListingType,
                        Status = p.Status,
                        CreatedOn = p.CreatedOn
                    })
                    .ToList(),
                TopAgents = response.TopAgents
                    .Select(a => new DashboardAgentViewModel
                    {
                        Id = a.Id,
                        FullName = $"{a.FirstName} {a.LastName}",
                        PropertyCount = a.PropertyCount
                    })
                    .ToList()
            };

            return View(model);
        }
    }
}
