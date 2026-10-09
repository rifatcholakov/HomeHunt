using HomeHunt.ViewModels;

namespace HomeHunt.Services
{
    public interface ILocationService
    {
        Task<List<LocationListViewModel>> GetAllAsync();

        Task CreateAsync(LocationFormViewModel model);
    }
}
