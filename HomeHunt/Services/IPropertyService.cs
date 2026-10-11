using HomeHunt.ViewModels;

namespace HomeHunt.Services
{
    public interface IPropertyService
    {
        Task<List<PropertyListViewModel>> GetAllAsync();

        Task<PropertyDetailsViewModel?> GetByIdAsync(int id);

        Task<PropertyFormViewModel?> GetEditModelAsync(int id);

        Task CreateAsync(PropertyFormViewModel model);

        Task<bool> UpdateAsync(int id, PropertyFormViewModel model);

        Task<bool> DeleteAsync(int id);
    }
}
