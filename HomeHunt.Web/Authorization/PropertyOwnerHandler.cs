using System.Security.Claims;
using HomeHunt.Common;
using Microsoft.AspNetCore.Authorization;

namespace HomeHunt.Authorization
{
    public class PropertyOwnerHandler : AuthorizationHandler<PropertyOwnerRequirement, string>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PropertyOwnerRequirement requirement,
            string ownerUserId)
        {
            if (context.User.IsInRole(Roles.Administrator))
            {
                context.Succeed(requirement);
            }
            else if (context.User.IsInRole(Roles.Agent)
                && context.User.FindFirstValue(ClaimTypes.NameIdentifier) == ownerUserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
