using System.ComponentModel.DataAnnotations;
using static HomeHunt.Common.Messages.Account;
using static HomeHunt.Common.ValidationConstants.Account;

namespace HomeHunt.ViewModels
{
    public class AgentCreateViewModel : AgentFormViewModel
    {
        [Required(ErrorMessage = PasswordRequiredMessage)]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = PasswordLengthMessage)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
