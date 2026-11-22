using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static HomeHunt.Common.Messages.Agent;
using static HomeHunt.Common.ValidationConstants.Agent;

namespace HomeHunt.Data.Models
{
    public class Agent
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = FirstNameRequiredMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = FirstNameLengthMessage)]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = LastNameRequiredMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = LastNameLengthMessage)]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = PhoneRequiredMessage)]
        [Phone(ErrorMessage = PhoneInvalidMessage)]
        [StringLength(PhoneMaxLength)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(AgencyNameMaxLength, MinimumLength = AgencyNameMinLength,
            ErrorMessage = AgencyNameLengthMessage)]
        [Display(Name = "Agency")]
        public string? AgencyName { get; set; }

        public string UserId { get; set; } = string.Empty;

        public IdentityUser User { get; set; } = null!;

        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
