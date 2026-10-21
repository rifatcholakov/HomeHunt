using HomeHunt.Common.Enums;

namespace HomeHunt.Service.Data.Properties.Response
{
    public class GetPropertyDetailsResponseData
    {
        public int Id { get; set; }

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

        public DateTime CreatedOn { get; set; }

        public int LocationId { get; set; }

        public string City { get; set; } = string.Empty;

        public string Neighborhood { get; set; } = string.Empty;

        public int AgentId { get; set; }

        public string AgentFirstName { get; set; } = string.Empty;

        public string AgentLastName { get; set; } = string.Empty;

        public string AgentEmail { get; set; } = string.Empty;

        public string AgentPhone { get; set; } = string.Empty;
    }
}
