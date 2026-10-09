using System.ComponentModel.DataAnnotations;
using static HomeHunt.Common.ValidationConstants.Location;

namespace HomeHunt.Data.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength,
            ErrorMessage = "City must be between {2} and {1} characters.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Neighborhood is required.")]
        [StringLength(NeighborhoodMaxLength, MinimumLength = NeighborhoodMinLength,
            ErrorMessage = "Neighborhood must be between {2} and {1} characters.")]
        public string Neighborhood { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is required.")]
        [RegularExpression(PostalCodeRegex, ErrorMessage = "Postal code must contain exactly 4 digits.")]
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; } = string.Empty;

        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
