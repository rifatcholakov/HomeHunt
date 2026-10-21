using HomeHunt.Common.Enums;

namespace HomeHunt.Service.Data.Properties.Request
{
    public class SearchPropertiesRequestData
    {
        public string? City { get; set; }

        public PropertyType? PropertyType { get; set; }

        public ListingType? ListingType { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public int? MinBedrooms { get; set; }

        public decimal? MinArea { get; set; }

        public string SortBy { get; set; } = "newest";
    }
}
