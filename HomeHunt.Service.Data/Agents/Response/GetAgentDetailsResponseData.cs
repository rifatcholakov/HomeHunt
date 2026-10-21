using HomeHunt.Service.Data.Properties.Response;

namespace HomeHunt.Service.Data.Agents.Response
{
    public class GetAgentDetailsResponseData
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string AgencyName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public int PropertyCount { get; set; }

        public int ForSaleCount { get; set; }

        public int ForRentCount { get; set; }

        public List<GetPropertiesResponseData> Properties { get; set; } = new List<GetPropertiesResponseData>();
    }
}
