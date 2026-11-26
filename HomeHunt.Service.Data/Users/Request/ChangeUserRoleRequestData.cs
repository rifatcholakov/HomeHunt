namespace HomeHunt.Service.Data.Users.Request
{
    public class ChangeUserRoleRequestData
    {
        public string UserId { get; set; } = string.Empty;

        public string NewRole { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string? AgencyName { get; set; }
    }
}
