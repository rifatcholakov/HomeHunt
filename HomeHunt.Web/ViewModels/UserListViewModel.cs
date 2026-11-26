namespace HomeHunt.ViewModels
{
    public class UserListViewModel
    {
        public string Id { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new List<string>();

        public bool IsAdministrator => Roles.Contains(Common.Roles.Administrator);

        public int? AgentId { get; set; }

        public int PropertyCount { get; set; }

        public bool IsCurrentUser { get; set; }
    }
}
