using HomeHunt.Service.Data.Locations.Response;
using HomeHunt.ViewModels;

namespace HomeHunt.Mapping
{
    public static class LocationMappings
    {
        public static LocationDetailsViewModel ToDetailsViewModel(this GetLocationDetailsResponseData location)
        {
            return new LocationDetailsViewModel
            {
                Id = location.Id,
                City = location.City,
                Neighborhood = location.Neighborhood,
                PostalCode = location.PostalCode,
                PropertyCount = location.PropertyCount,
                ForSaleCount = location.ForSaleCount,
                ForRentCount = location.ForRentCount,
                Properties = location.Properties
                    .Select(property => property.ToViewModel())
                    .ToList()
            };
        }
    }
}
