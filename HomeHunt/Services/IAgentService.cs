using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeHunt.Services
{
    public interface IAgentService
    {
        Task<List<AgentListViewModel>> GetAllAsync();

        Task<AgentDetailsViewModel?> GetByIdAsync(int id);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<List<SelectListItem>> GetOptionsAsync();

        Task<bool> EmailExistsAsync(string email);

        Task<bool> PhoneExistsAsync(string phone);

        Task CreateAsync(AgentFormViewModel model);
    }
}
