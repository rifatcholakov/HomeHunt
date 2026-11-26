using HomeHunt.Common.Enums;

namespace HomeHunt.ViewModels
{
    public class DashboardPropertyViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public ListingType ListingType { get; set; }

        public PropertyStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
