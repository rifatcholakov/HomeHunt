namespace HomeHunt.Service.Data.Agents.Response
{
    public class RegisterAgentResponseData
    {
        public bool Succeeded { get; set; }

        public string? UserId { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
