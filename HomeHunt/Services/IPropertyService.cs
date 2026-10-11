using HomeHunt.ViewModels;

namespace HomeHunt.Services
{
    public interface IPropertyService
    {
        Task<List<PropertyListViewModel>> GetAllAsync();
    }
}
