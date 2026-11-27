using HomeHunt.Service.Data.Agents.Request;
using HomeHunt.Service.Data.Agents.Response;

namespace HomeHunt.Service.Data.Agents
{
    public interface IAgentService
    {
        Task<PagedResponseData<GetAllAgentsResponseData>> GetAllAgentsAsync(int page, int pageSize);

        Task<GetAgentDetailsResponseData?> GetAgentDetailsAsync(int id);

        Task<bool> DeleteAgentAsync(int id);

        Task<bool> AgentExistsAsync(int id);

        Task<int?> GetAgentIdByUserIdAsync(string userId);

        Task<IEnumerable<GetAgentOptionsResponseData>> GetAgentOptionsAsync();

        Task<bool> EmailExistsAsync(string email);

        Task<bool> PhoneExistsAsync(string phone, int excludeId = 0);

        Task<GetAgentForEditResponseData?> GetAgentForEditAsync(int id);

        Task<OperationResponseData> CreateAgentAsync(CreateAgentRequestData request);

        Task<OperationResponseData> UpgradeToAgentAsync(UpgradeToAgentRequestData request);

        Task<bool> UpdateAgentAsync(int id, AgentProfileRequestData request);
    }
}
