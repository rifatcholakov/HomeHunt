using HomeHunt.ViewModels;

namespace HomeHunt.Services
{
    public interface IPropertyService
    {
        Task<List<PropertyListViewModel>> SearchAsync(PropertySearchViewModel searchModel);

        Task<List<PropertyListViewModel>> GetRecentAsync(int count);

        Task<int> CountAvailableAsync();

        Task<int> CountAvailableForSaleAsync();

        Task<int> CountAvailableForRentAsync();

        Task<PropertyDetailsViewModel?> GetByIdAsync(int id);

        Task<PropertyFormViewModel?> GetEditModelAsync(int id);

        Task CreateAsync(PropertyFormViewModel model);

        Task<bool> UpdateAsync(int id, PropertyFormViewModel model);

        Task<bool> DeleteAsync(int id);
    }
}
