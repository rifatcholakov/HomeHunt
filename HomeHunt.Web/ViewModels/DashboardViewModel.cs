namespace HomeHunt.ViewModels
{
    public class DashboardViewModel
    {
        public int PropertyCount { get; set; }

        public int AvailablePropertyCount { get; set; }

        public int AgentCount { get; set; }

        public int LocationCount { get; set; }

        public int UserCount { get; set; }

        public int FavoriteCount { get; set; }

        public int ForSaleCount { get; set; }

        public int ForRentCount { get; set; }

        public List<StatusCountViewModel> StatusCounts { get; set; } = new List<StatusCountViewModel>();

        public List<DashboardPropertyViewModel> RecentProperties { get; set; } = new List<DashboardPropertyViewModel>();

        public List<DashboardAgentViewModel> TopAgents { get; set; } = new List<DashboardAgentViewModel>();

        public int Percentage(int count)
        {
            return PropertyCount == 0 ? 0 : count * 100 / PropertyCount;
        }
    }
}
