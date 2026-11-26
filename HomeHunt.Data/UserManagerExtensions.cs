using Microsoft.AspNetCore.Identity;

namespace HomeHunt.Data
{
    public static class UserManagerExtensions
    {
        public static async Task<IdentityResult> SetSingleRoleAsync(
            this UserManager<IdentityUser> userManager,
            IdentityUser user,
            string role)
        {
            IList<string> currentRoles = await userManager.GetRolesAsync(user);
            string[] rolesToRemove = currentRoles.Where(r => r != role).ToArray();

            if (rolesToRemove.Length > 0)
            {
                IdentityResult removeResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);

                if (!removeResult.Succeeded)
                {
                    return removeResult;
                }
            }

            if (!currentRoles.Contains(role))
            {
                return await userManager.AddToRoleAsync(user, role);
            }

            return IdentityResult.Success;
        }
    }
}
