using HomeHunt.ViewModels;

namespace HomeHunt.Services
{
    public interface IAgentService
    {
        Task<List<AgentListViewModel>> GetAllAsync();

        Task<AgentDetailsViewModel?> GetByIdAsync(int id);

        Task<bool> EmailExistsAsync(string email);

        Task<bool> PhoneExistsAsync(string phone);

        Task CreateAsync(AgentFormViewModel model);
    }
}
