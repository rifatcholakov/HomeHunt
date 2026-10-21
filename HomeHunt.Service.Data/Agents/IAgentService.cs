using HomeHunt.Service.Data.Agents.Request;
using HomeHunt.Service.Data.Agents.Response;

namespace HomeHunt.Service.Data.Agents
{
    public interface IAgentService
    {
        Task<IEnumerable<GetAllAgentsResponseData>> GetAllAgentsAsync();

        Task<GetAgentDetailsResponseData?> GetAgentDetailsAsync(int id);

        Task<bool> DeleteAgentAsync(int id);

        Task<bool> AgentExistsAsync(int id);

        Task<IEnumerable<GetAgentOptionsResponseData>> GetAgentOptionsAsync();

        Task<bool> EmailExistsAsync(string email, int excludeId = 0);

        Task<bool> PhoneExistsAsync(string phone, int excludeId = 0);

        Task<GetAgentForEditResponseData?> GetAgentForEditAsync(int id);

        Task CreateAgentAsync(CreateAgentRequestData request);

        Task<bool> UpdateAgentAsync(int id, UpdateAgentRequestData request);
    }
}
