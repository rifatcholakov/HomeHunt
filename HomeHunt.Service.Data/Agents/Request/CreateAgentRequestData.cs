namespace HomeHunt.Service.Data.Agents.Request
{
    public class CreateAgentRequestData
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string AgencyName { get; set; } = string.Empty;
    }
}
