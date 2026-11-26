namespace HomeHunt.Service.Data.Users.Response
{
    public class GetUserResponseData
    {
        public string Id { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new List<string>();

        public int? AgentId { get; set; }

        public int PropertyCount { get; set; }
    }
}
