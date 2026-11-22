namespace HomeHunt.Service.Data.Agents.Response
{
    public class CreateAgentResponseData
    {
        public bool Succeeded { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
