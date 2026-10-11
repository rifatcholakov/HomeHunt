using HomeHunt.ViewModels;

namespace HomeHunt.Services
{
    public interface IPropertyService
    {
        Task<List<PropertyListViewModel>> GetAllAsync();

        Task<PropertyDetailsViewModel?> GetByIdAsync(int id);

        Task CreateAsync(PropertyFormViewModel model);
    }
}
