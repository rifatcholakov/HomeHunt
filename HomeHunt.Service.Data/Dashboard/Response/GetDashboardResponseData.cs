namespace HomeHunt.Service.Data.Dashboard.Response
{
    public class GetDashboardResponseData
    {
        public int PropertyCount { get; set; }

        public int AvailablePropertyCount { get; set; }

        public int AgentCount { get; set; }

        public int LocationCount { get; set; }

        public int UserCount { get; set; }

        public int FavoriteCount { get; set; }

        public int ForSaleCount { get; set; }

        public int ForRentCount { get; set; }

        public List<GetStatusCountResponseData> StatusCounts { get; set; } = new List<GetStatusCountResponseData>();

        public List<GetDashboardPropertyResponseData> RecentProperties { get; set; } = new List<GetDashboardPropertyResponseData>();

        public List<GetDashboardAgentResponseData> TopAgents { get; set; } = new List<GetDashboardAgentResponseData>();
    }
}
