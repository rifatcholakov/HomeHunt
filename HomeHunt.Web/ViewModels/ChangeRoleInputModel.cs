using System.ComponentModel.DataAnnotations;
using HomeHunt.Common;
using static HomeHunt.Common.Messages.Agent;
using static HomeHunt.Common.ValidationConstants.Agent;

namespace HomeHunt.ViewModels
{
    public class ChangeRoleInputModel
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _phone = string.Empty;
        private string? _agencyName;

        [Required]
        [Display(Name = "New role")]
        public string NewRole { get; set; } = string.Empty;

        [Required(ErrorMessage = FirstNameRequiredMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = FirstNameLengthMessage)]
        [Display(Name = "First name")]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = Helpers.NormalizeWhitespace(value); }
        }

        [Required(ErrorMessage = LastNameRequiredMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = LastNameLengthMessage)]
        [Display(Name = "Last name")]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = Helpers.NormalizeWhitespace(value); }
        }

        [Required(ErrorMessage = PhoneRequiredMessage)]
        [RegularExpression(PhoneRegex, ErrorMessage = PhoneFormatMessage)]
        [StringLength(PhoneMaxLength, ErrorMessage = PhoneMaxLengthMessage)]
        public string Phone
        {
            get { return _phone; }
            set { _phone = Helpers.NormalizePhoneNumber(value); }
        }

        [StringLength(AgencyNameMaxLength, MinimumLength = AgencyNameMinLength,
            ErrorMessage = AgencyNameLengthMessage)]
        [Display(Name = "Agency")]
        public string? AgencyName
        {
            get { return _agencyName; }
            set
            {
                string normalized = Helpers.NormalizeWhitespace(value);
                _agencyName = normalized.Length == 0 ? null : normalized;
            }
        }
    }
}
