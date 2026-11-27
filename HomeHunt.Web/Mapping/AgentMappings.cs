using HomeHunt.Service.Data.Agents.Response;
using HomeHunt.ViewModels;

namespace HomeHunt.Mapping
{
    public static class AgentMappings
    {
        public static AgentDetailsViewModel ToDetailsViewModel(this GetAgentDetailsResponseData agent)
        {
            return new AgentDetailsViewModel
            {
                Id = agent.Id,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                AgencyName = agent.AgencyName,
                Email = agent.Email,
                Phone = agent.Phone,
                PropertyCount = agent.PropertyCount,
                ForSaleCount = agent.ForSaleCount,
                ForRentCount = agent.ForRentCount,
                Properties = agent.Properties
                    .Select(property => property.ToViewModel())
                    .ToList()
            };
        }
    }
}
