namespace HomeHunt.Service.Data.Agents.Request
{
    public class CreateAgentRequestData : AgentProfileRequestData
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
