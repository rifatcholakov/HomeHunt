using System.ComponentModel.DataAnnotations;
using HomeHunt.Common;
using static HomeHunt.Common.ValidationConstants.Agent;

namespace HomeHunt.ViewModels
{
    public class AgentFormViewModel
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        private string _agencyName = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = "First name must be between {2} and {1} characters.")]
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

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = "Last name must be between {2} and {1} characters.")]
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

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(EmailMaxLength, ErrorMessage = "Email can be at most {1} characters.")]
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

        [Required(ErrorMessage = "Phone is required.")]
        [RegularExpression(PhoneRegex, ErrorMessage = "Phone must start with + and contain 7 to 15 digits.")]
        [StringLength(PhoneMaxLength, ErrorMessage = "Phone can be at most {1} characters.")]
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

        [Required(ErrorMessage = "Agency name is required.")]
        [StringLength(AgencyNameMaxLength, MinimumLength = AgencyNameMinLength,
            ErrorMessage = "Agency name must be between {2} and {1} characters.")]
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
