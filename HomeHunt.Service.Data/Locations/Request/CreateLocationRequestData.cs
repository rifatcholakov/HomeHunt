namespace HomeHunt.Service.Data.Locations.Request
{
    public class CreateLocationRequestData
    {
        public string City { get; set; } = string.Empty;

        public string Neighborhood { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;
    }
}
