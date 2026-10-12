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

        Task<bool> EmailExistsAsync(string email, int excludeId = 0);

        Task<bool> PhoneExistsAsync(string phone, int excludeId = 0);

        Task<AgentFormViewModel?> GetEditModelAsync(int id);

        Task CreateAsync(AgentFormViewModel model);

        Task<bool> UpdateAsync(int id, AgentFormViewModel model);
    }
}
