using System.ComponentModel.DataAnnotations;
using static HomeHunt.Common.ValidationConstants.Agent;

namespace HomeHunt.Data.Models
{
    public class Agent
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = "First name must be between {2} and {1} characters.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = "Last name must be between {2} and {1} characters.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(EmailMaxLength)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [StringLength(PhoneMaxLength)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Agency name is required.")]
        [StringLength(AgencyNameMaxLength, MinimumLength = AgencyNameMinLength,
            ErrorMessage = "Agency name must be between {2} and {1} characters.")]
        [Display(Name = "Agency")]
        public string AgencyName { get; set; } = string.Empty;

        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
