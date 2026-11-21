using System.ComponentModel.DataAnnotations;
using static HomeHunt.Common.Messages.Location;
using static HomeHunt.Common.ValidationConstants.Location;

namespace HomeHunt.Data.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = CityRequiredMessage)]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength,
            ErrorMessage = CityLengthMessage)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = NeighborhoodRequiredMessage)]
        [StringLength(NeighborhoodMaxLength, MinimumLength = NeighborhoodMinLength,
            ErrorMessage = NeighborhoodLengthMessage)]
        public string Neighborhood { get; set; } = string.Empty;

        [Required(ErrorMessage = PostalCodeRequiredMessage)]
        [RegularExpression(PostalCodeRegex, ErrorMessage = PostalCodeFormatMessage)]
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; } = string.Empty;

        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
