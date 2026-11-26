using System.Security.Claims;
using HomeHunt.Common;

namespace HomeHunt.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsSignedIn(this ClaimsPrincipal user) => user.Identity?.IsAuthenticated == true;

        public static bool IsAdmin(this ClaimsPrincipal user) => user.IsInRole(Roles.Administrator);

        public static bool IsAgent(this ClaimsPrincipal user) => user.IsInRole(Roles.Agent);

        public static string GetRole(this ClaimsPrincipal user) =>
            user.IsAdmin() ? Roles.Administrator
            : user.IsAgent() ? Roles.Agent
            : Roles.User;
    }
}
