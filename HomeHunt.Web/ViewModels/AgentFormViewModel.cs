using System.ComponentModel.DataAnnotations;
using HomeHunt.Common;
using static HomeHunt.Common.ValidationConstants.Agent;
using static HomeHunt.Common.Messages.Agent;
using static HomeHunt.Common.Messages.User;

namespace HomeHunt.ViewModels
{
    public class AgentFormViewModel
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        private string _agencyName = string.Empty;

        public int Id { get; set; }

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
    }
}
