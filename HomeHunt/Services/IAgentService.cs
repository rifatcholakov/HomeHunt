using HomeHunt.ViewModels;

namespace HomeHunt.Services
{
    public interface IAgentService
    {
        Task<List<AgentListViewModel>> GetAllAsync();

        Task<bool> EmailExistsAsync(string email);

        Task<bool> PhoneExistsAsync(string phone);

        Task CreateAsync(AgentFormViewModel model);
    }
}
