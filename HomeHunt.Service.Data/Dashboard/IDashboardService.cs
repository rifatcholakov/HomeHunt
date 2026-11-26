using HomeHunt.Service.Data.Dashboard.Response;

namespace HomeHunt.Service.Data.Dashboard
{
    public interface IDashboardService
    {
        Task<GetDashboardResponseData> GetDashboardAsync();
    }
}
