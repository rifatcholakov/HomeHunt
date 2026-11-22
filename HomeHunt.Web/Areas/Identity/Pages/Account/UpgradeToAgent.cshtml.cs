using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeHunt.Areas.Identity.Pages.Account
{
    [Authorize]
    public class UpgradeToAgentModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
