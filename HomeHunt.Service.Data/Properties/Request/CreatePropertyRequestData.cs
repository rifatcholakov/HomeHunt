using HomeHunt.Common.Enums;

namespace HomeHunt.Service.Data.Properties.Request
{
    public class CreatePropertyRequestData
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal Area { get; set; }

        public int Bedrooms { get; set; }

        public int Bathrooms { get; set; }

        public PropertyType PropertyType { get; set; }

        public ListingType ListingType { get; set; }

        public PropertyStatus Status { get; set; }

        public string? ImageUrl { get; set; }

        public int LocationId { get; set; }

        public int AgentId { get; set; }
    }
}
