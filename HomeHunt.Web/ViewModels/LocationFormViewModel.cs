using System.ComponentModel.DataAnnotations;
using HomeHunt.Common;
using static HomeHunt.Common.ValidationConstants.Location;
using static HomeHunt.Common.Messages.Location;

namespace HomeHunt.ViewModels
{
    public class LocationFormViewModel
    {
        private string _city = string.Empty;
        private string _neighborhood = string.Empty;
        private string _postalCode = string.Empty;

        public int Id { get; set; }

        [Required(ErrorMessage = CityRequiredMessage)]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength,
            ErrorMessage = CityLengthMessage)]
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

        [Required(ErrorMessage = NeighborhoodRequiredMessage)]
        [StringLength(NeighborhoodMaxLength, MinimumLength = NeighborhoodMinLength,
            ErrorMessage = NeighborhoodLengthMessage)]
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

        [Required(ErrorMessage = PostalCodeRequiredMessage)]
        [RegularExpression(PostalCodeRegex, ErrorMessage = PostalCodeFormatMessage)]
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
