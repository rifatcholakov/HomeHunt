using HomeHunt.Service.Data.Properties.Response;

namespace HomeHunt.Service.Data.Locations.Response
{
    public class GetLocationDetailsResponseData
    {
        public int Id { get; set; }

        public string City { get; set; } = string.Empty;

        public string Neighborhood { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public int PropertyCount { get; set; }

        public int ForSaleCount { get; set; }

        public int ForRentCount { get; set; }

        public List<GetPropertiesResponseData> Properties { get; set; } = new List<GetPropertiesResponseData>();
    }
}
