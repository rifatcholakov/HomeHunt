using HomeHunt.Service.Data.Properties.Request;
using HomeHunt.Service.Data.Properties.Response;

namespace HomeHunt.Service.Data.Properties
{
    public interface IPropertyService
    {
        Task<PagedResponseData<GetPropertiesResponseData>> SearchPropertiesAsync(SearchPropertiesRequestData request);

        Task<IEnumerable<GetPropertiesResponseData>> GetRecentPropertiesAsync(int count);

        Task<int> CountAvailablePropertiesAsync();

        Task<int> CountAvailablePropertiesForSaleAsync();

        Task<int> CountAvailablePropertiesForRentAsync();

        Task<GetPropertyDetailsResponseData?> GetPropertyDetailsAsync(int id);

        Task<GetPropertyForEditResponseData?> GetPropertyForEditAsync(int id);

        Task<string?> GetOwnerUserIdAsync(int propertyId);

        Task<OperationResponseData> CreatePropertyAsync(SavePropertyRequestData request, CancellationToken cancellationToken = default);

        Task<OperationResponseData> UpdatePropertyAsync(int id, UpdatePropertyRequestData request, CancellationToken cancellationToken = default);

        Task<bool> DeletePropertyAsync(int id, CancellationToken cancellationToken = default);
    }
}
