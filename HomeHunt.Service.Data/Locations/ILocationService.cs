using HomeHunt.Service.Data.Locations.Request;
using HomeHunt.Service.Data.Locations.Response;

namespace HomeHunt.Service.Data.Locations
{
    public interface ILocationService
    {
        Task<IEnumerable<GetAllLocationsResponseData>> GetAllLocationsAsync();

        Task<GetLocationDetailsResponseData?> GetLocationDetailsAsync(int id);

        Task<bool> LocationExistsAsync(int id);

        Task<bool> LocationExistsAsync(string city, string neighborhood, int excludeId = 0);

        Task<IEnumerable<GetLocationOptionsResponseData>> GetLocationOptionsAsync();

        Task<IEnumerable<string>> GetCitiesAsync();

        Task<IEnumerable<GetCityCountsResponseData>> GetCityCountsAsync();

        Task<string?> GetCityByPostalCodeAsync(string postalCode, int excludeId = 0);

        Task<GetLocationForEditResponseData?> GetLocationForEditAsync(int id);

        Task CreateLocationAsync(CreateLocationRequestData request);

        Task<bool> UpdateLocationAsync(int id, UpdateLocationRequestData request);

        Task<bool> DeleteLocationAsync(int id);
    }
}
