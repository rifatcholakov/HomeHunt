using HomeHunt.Service.Data.Properties.Request;
using HomeHunt.Service.Data.Properties.Response;

namespace HomeHunt.Service.Data.Properties
{
    public interface IPropertyService
    {
        Task<IEnumerable<GetPropertiesResponseData>> SearchPropertiesAsync(SearchPropertiesRequestData request);

        Task<IEnumerable<GetPropertiesResponseData>> GetRecentPropertiesAsync(int count);

        Task<int> CountAvailablePropertiesAsync();

        Task<int> CountAvailablePropertiesForSaleAsync();

        Task<int> CountAvailablePropertiesForRentAsync();

        Task<GetPropertyDetailsResponseData?> GetPropertyDetailsAsync(int id);

        Task<GetPropertyForEditResponseData?> GetPropertyForEditAsync(int id);

        Task CreatePropertyAsync(CreatePropertyRequestData request);

        Task<bool> UpdatePropertyAsync(int id, UpdatePropertyRequestData request);

        Task<bool> DeletePropertyAsync(int id);
    }
}
