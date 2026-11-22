using HomeHunt.Service.Data.Agents;
using HomeHunt.Service.Data.Agents.Request;
using HomeHunt.Service.Data.Agents.Response;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static HomeHunt.Common.Messages.Account;
using static HomeHunt.Common.Messages.Agent;

namespace HomeHunt.Areas.Identity.Pages.Account
{
    public class RegisterAgentModel : PageModel
    {
        private readonly IAgentService _agentService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public RegisterAgentModel(
            IAgentService agentService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _agentService = agentService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public AgentRegisterViewModel Input { get; set; } = new AgentRegisterViewModel();

        public string? ReturnUrl { get; set; }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (await _agentService.EmailExistsAsync(Input.Email))
            {
                ModelState.AddModelError($"{nameof(Input)}.{nameof(Input.Email)}", EmailTakenMessage);
            }

            if (await _agentService.PhoneExistsAsync(Input.Phone))
            {
                ModelState.AddModelError($"{nameof(Input)}.{nameof(Input.Phone)}", PhoneTakenMessage);
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            CreateAgentResponseData result = await _agentService.CreateAgentAsync(new CreateAgentRequestData
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Email = Input.Email,
                Phone = Input.Phone,
                AgencyName = Input.AgencyName,
                Password = Input.Password
            });

            if (!result.Succeeded)
            {
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return Page();
            }

            IdentityUser? user = await _userManager.FindByEmailAsync(Input.Email);

            if (user == null)
            {
                return RedirectToPage("./Login", new { returnUrl });
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return LocalRedirect(returnUrl);
        }
    }
}
