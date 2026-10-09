namespace HomeHunt.ViewModels
{
    public class LocationListViewModel
    {
        public int Id { get; set; }

        public string City { get; set; } = string.Empty;

        public string Neighborhood { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public int PropertyCount { get; set; }
    }
}
