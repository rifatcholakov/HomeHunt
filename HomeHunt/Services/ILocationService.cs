using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeHunt.Services
{
    public interface ILocationService
    {
        Task<List<LocationListViewModel>> GetAllAsync();

        Task<LocationDetailsViewModel?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<bool> ExistsAsync(string city, string neighborhood, int excludeId = 0);

        Task<List<SelectListItem>> GetOptionsAsync();

        Task<List<string>> GetCitiesAsync();

        Task<List<CityCountViewModel>> GetCityCountsAsync();

        Task<string?> GetCityByPostalCodeAsync(string postalCode, int excludeId = 0);

        Task<LocationFormViewModel?> GetEditModelAsync(int id);

        Task CreateAsync(LocationFormViewModel model);

        Task<bool> UpdateAsync(int id, LocationFormViewModel model);

        Task<bool> DeleteAsync(int id);
    }
}
