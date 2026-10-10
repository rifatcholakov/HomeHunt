using System.ComponentModel.DataAnnotations;
using HomeHunt.Common;
using static HomeHunt.Common.ValidationConstants.Location;

namespace HomeHunt.ViewModels
{
    public class LocationFormViewModel
    {
        private string _city = string.Empty;
        private string _neighborhood = string.Empty;
        private string _postalCode = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength,
            ErrorMessage = "City must be between {2} and {1} characters.")]
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = Helpers.Normalize(value);
            }
        }

        [Required(ErrorMessage = "Neighborhood is required.")]
        [StringLength(NeighborhoodMaxLength, MinimumLength = NeighborhoodMinLength,
            ErrorMessage = "Neighborhood must be between {2} and {1} characters.")]
        public string Neighborhood
        {
            get
            {
                return _neighborhood;
            }
            set
            {
                _neighborhood = Helpers.Normalize(value);
            }
        }

        [Required(ErrorMessage = "Postal code is required.")]
        [RegularExpression(PostalCodeRegex, ErrorMessage = "Postal code must contain exactly 4 digits.")]
        [Display(Name = "Postal code")]
        public string PostalCode
        {
            get
            {
                return _postalCode;
            }
            set
            {
                _postalCode = Helpers.Normalize(value);
            }
        }
    }
}
