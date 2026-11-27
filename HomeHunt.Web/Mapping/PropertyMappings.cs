using HomeHunt.Service.Data.Properties.Response;
using HomeHunt.ViewModels;

namespace HomeHunt.Mapping
{
    public static class PropertyMappings
    {
        public static PropertyListViewModel ToViewModel(this GetPropertiesResponseData property)
        {
            return new PropertyListViewModel
            {
                Id = property.Id,
                Title = property.Title,
                City = property.City,
                Neighborhood = property.Neighborhood,
                Price = property.Price,
                Area = property.Area,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                PropertyType = property.PropertyType,
                ListingType = property.ListingType,
                Status = property.Status,
                ImageUrl = property.ImageUrl,
                OwnerUserId = property.OwnerUserId
            };
        }

        public static PropertyDetailsViewModel ToViewModel(this GetPropertyDetailsResponseData property)
        {
            return new PropertyDetailsViewModel
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                Price = property.Price,
                Area = property.Area,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                PropertyType = property.PropertyType,
                ListingType = property.ListingType,
                Status = property.Status,
                CreatedOn = property.CreatedOn,
                LocationId = property.LocationId,
                City = property.City,
                Neighborhood = property.Neighborhood,
                AgentId = property.AgentId,
                OwnerUserId = property.OwnerUserId,
                AgentFirstName = property.AgentFirstName,
                AgentLastName = property.AgentLastName,
                AgentEmail = property.AgentEmail,
                AgentPhone = property.AgentPhone,
                Images = PropertyImageFormMapper.ToViewModels(property.Images)
            };
        }
    }
}
