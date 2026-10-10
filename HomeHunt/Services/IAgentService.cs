using HomeHunt.ViewModels;

namespace HomeHunt.Services
{
    public interface IAgentService
    {
        Task<List<AgentListViewModel>> GetAllAsync();
    }
}
