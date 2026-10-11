namespace HomeHunt.ViewModels
{
    public class HomeViewModel
    {
        public int AvailableCount { get; set; }

        public int ForSaleCount { get; set; }

        public int ForRentCount { get; set; }

        public List<PropertyListViewModel> RecentProperties { get; set; } = new List<PropertyListViewModel>();

        public List<string> Cities { get; set; } = new List<string>();

        public List<CityCountViewModel> CityCounts { get; set; } = new List<CityCountViewModel>();
    }
}
