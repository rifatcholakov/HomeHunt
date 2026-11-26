using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeHunt.ViewModels
{
    public class ChangeRoleViewModel
    {
        public string UserId { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string CurrentRole { get; set; } = string.Empty;

        public bool HasAgentProfile { get; set; }

        public List<SelectListItem> RoleOptions { get; set; } = new List<SelectListItem>();

        public ChangeRoleInputModel Input { get; set; } = new ChangeRoleInputModel();
    }
}
