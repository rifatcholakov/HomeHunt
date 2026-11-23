using HomeHunt.Service.Data.Properties.Response;

namespace HomeHunt.Service.Data.Favorites
{
    public interface IFavoriteService
    {
        Task<bool> AddFavoriteAsync(string userId, int propertyId);

        Task RemoveFavoriteAsync(string userId, int propertyId);

        Task<HashSet<int>> GetFavoritePropertyIdsAsync(string userId);

        Task<IEnumerable<GetPropertiesResponseData>> GetFavoritePropertiesAsync(string userId);
    }
}
