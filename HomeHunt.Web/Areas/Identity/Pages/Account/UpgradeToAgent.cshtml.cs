using System.Security.Claims;
using HomeHunt.Common;
using HomeHunt.Controllers;
using HomeHunt.Service.Data.Agents;
using HomeHunt.Service.Data.Agents.Request;
using HomeHunt.Service.Data.Agents.Response;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static HomeHunt.Common.Messages.Account;
using static HomeHunt.Common.Messages.Agent;

namespace HomeHunt.Areas.Identity.Pages.Account
{
    [Authorize]
    public class UpgradeToAgentModel : PageModel
    {
        private readonly IAgentService _agentService;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UpgradeToAgentModel(
            IAgentService agentService,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _agentService = agentService;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public AgentUpgradeViewModel Input { get; set; } = new AgentUpgradeViewModel();

        public string Email { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            if (await IsAlreadyAgentAsync())
            {
                return RedirectToCreateProperty();
            }

            Email = User.Identity?.Name ?? string.Empty;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (await IsAlreadyAgentAsync())
            {
                return RedirectToCreateProperty();
            }

            Email = User.Identity?.Name ?? string.Empty;
            ModelState.Remove($"{nameof(Input)}.{nameof(Input.Email)}");

            if (await _agentService.PhoneExistsAsync(Input.Phone))
            {
                ModelState.AddModelError($"{nameof(Input)}.{nameof(Input.Phone)}", PhoneTakenMessage);
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            CreateAgentResponseData result = await _agentService.UpgradeToAgentAsync(new UpgradeToAgentRequestData
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Phone = Input.Phone,
                AgencyName = Input.AgencyName
            });

            if (!result.Succeeded)
            {
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return Page();
            }

            IdentityUser? user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                await _signInManager.RefreshSignInAsync(user);
            }

            TempData[TempDataKeys.SuccessMessage] = UpgradedToAgentMessage;
            return RedirectToCreateProperty();
        }

        private async Task<bool> IsAlreadyAgentAsync()
        {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, Policies.CanManageProperties);

            return result.Succeeded;
        }

        private IActionResult RedirectToCreateProperty()
        {
            return RedirectToAction(nameof(PropertiesController.Create), Routes.PropertiesController, new { area = string.Empty });
        }
    }
}
