using HomeHunt.Common.Enums;

namespace HomeHunt.Service.Data.Properties.Response
{
    public class GetPropertiesResponseData
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Neighborhood { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal Area { get; set; }

        public int Bedrooms { get; set; }

        public int Bathrooms { get; set; }

        public PropertyType PropertyType { get; set; }

        public ListingType ListingType { get; set; }

        public PropertyStatus Status { get; set; }

        public string? ImageUrl { get; set; }
    }
}
