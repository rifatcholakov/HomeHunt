namespace HomeHunt.Common
{
    public static class Roles
    {
        public const string Administrator = "Administrator";
        public const string Agent = "Agent";
        public const string User = "User";

        public static readonly IReadOnlyList<string> All = new[] { Administrator, Agent, User };
    }
}
