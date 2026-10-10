using HomeHunt.ViewModels;

namespace HomeHunt.Services
{
    public interface ILocationService
    {
        Task<List<LocationListViewModel>> GetAllAsync();

        Task<LocationDetailsViewModel?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(string city, string neighborhood);

        Task<string?> GetCityByPostalCodeAsync(string postalCode);

        Task CreateAsync(LocationFormViewModel model);
    }
}
