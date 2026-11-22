using System.ComponentModel.DataAnnotations;
using static HomeHunt.Common.Messages.Account;

namespace HomeHunt.ViewModels
{
    public class AgentRegisterViewModel : AgentCreateViewModel
    {
        [Required(ErrorMessage = ConfirmPasswordRequiredMessage)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare(nameof(Password), ErrorMessage = ConfirmPasswordMismatchMessage)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
