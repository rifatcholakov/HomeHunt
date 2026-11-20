using HomeHunt.Common;
using HomeHunt.Service.Data.Agents;
using HomeHunt.Service.Data.Agents.Request;
using HomeHunt.Service.Data.Agents.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using static HomeHunt.Common.Messages.Agent;
using static HomeHunt.Common.Messages.User;
using static HomeHunt.Common.ValidationConstants.User;
using static HomeHunt.Common.ValidationConstants.Agent;

namespace HomeHunt.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
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
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            private string _firstName = string.Empty;
            private string _lastName = string.Empty;
            private string _email = string.Empty;
            private string _phone = string.Empty;
            private string _agencyName = string.Empty;

            [Required(ErrorMessage = FirstNameRequiredMessage)]
            [StringLength(NameMaxLength, MinimumLength = NameMinLength,
                ErrorMessage = FirstNameLengthMessage)]
            [Display(Name = "First name")]
            public string FirstName
            {
                get
                {
                    return _firstName;
                }
                set
                {
                    _firstName = Helpers.Normalize(value);
                }
            }

            [Required(ErrorMessage = LastNameRequiredMessage)]
            [StringLength(NameMaxLength, MinimumLength = NameMinLength,
                ErrorMessage = LastNameLengthMessage)]
            [Display(Name = "Last name")]
            public string LastName
            {
                get
                {
                    return _lastName;
                }
                set
                {
                    _lastName = Helpers.Normalize(value);
                }
            }

            [Required(ErrorMessage = EmailRequiredMessage)]
            [EmailAddress(ErrorMessage = EmailInvalidMessage)]
            [StringLength(EmailMaxLength, ErrorMessage = EmailMaxLengthMessage)]
            public string Email
            {
                get
                {
                    return _email;
                }
                set
                {
                    _email = Helpers.Normalize(value);
                }
            }

            [Required(ErrorMessage = PhoneRequiredMessage)]
            [RegularExpression(PhoneRegex, ErrorMessage = PhoneFormatMessage)]
            [StringLength(PhoneMaxLength, ErrorMessage = PhoneMaxLengthMessage)]
            public string Phone
            {
                get
                {
                    return _phone;
                }
                set
                {
                    _phone = Helpers.NormalizePhone(value);
                }
            }

            [Required(ErrorMessage = AgencyNameRequiredMessage)]
            [StringLength(AgencyNameMaxLength, MinimumLength = AgencyNameMinLength,
                ErrorMessage = AgencyNameLengthMessage)]
            [Display(Name = "Agency")]
            public string AgencyName
            {
                get
                {
                    return _agencyName;
                }
                set
                {
                    _agencyName = Helpers.Normalize(value);
                }
            }

            [Required(ErrorMessage = PasswordRequiredMessage)]
            [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = PasswordLengthMessage)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare(nameof(Password), ErrorMessage = PasswordMismatchMessage)]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return LocalRedirect("~/");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (await _agentService.EmailExistsAsync(Input.Email.ToLowerInvariant()))
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

            RegisterAgentResponseData response = await _agentService.RegisterAgentAsync(new RegisterAgentRequestData
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Email = Input.Email,
                Phone = Input.Phone,
                AgencyName = Input.AgencyName,
                Password = Input.Password
            });

            if (!response.Succeeded)
            {
                foreach (string error in response.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return Page();
            }

            IdentityUser? user = await _userManager.FindByIdAsync(response.UserId!);

            if (user != null)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            TempData[TempDataKeys.SuccessMessage] = RegisteredMessage;
            return RedirectToAction("Create", "Properties", new { area = "" });
        }
    }
}
