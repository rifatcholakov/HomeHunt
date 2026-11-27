using System.Linq.Expressions;
using HomeHunt.Data.Models;
using HomeHunt.Service.Data.Properties.Response;

namespace HomeHunt.Services
{
    internal static class PropertyProjections
    {
        public static readonly Expression<Func<Property, GetPropertiesResponseData>> ListItem = p => new GetPropertiesResponseData
        {
            Id = p.Id,
            Title = p.Title,
            City = p.Location.City,
            Neighborhood = p.Location.Neighborhood,
            Price = p.Price,
            Area = p.Area,
            Bedrooms = p.Bedrooms,
            Bathrooms = p.Bathrooms,
            PropertyType = p.PropertyType,
            ListingType = p.ListingType,
            Status = p.Status,
            ImageUrl = p.Images
                .OrderBy(i => i.SortOrder)
                .ThenBy(i => i.Id)
                .Select(i => i.Url)
                .FirstOrDefault(),
            OwnerUserId = p.Agent.UserId
        };
    }
}
