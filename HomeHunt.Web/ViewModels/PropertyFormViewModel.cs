using System.ComponentModel.DataAnnotations;
using HomeHunt.Common;
using HomeHunt.Common.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using static HomeHunt.Common.ValidationConstants.Property;
using static HomeHunt.Common.Messages.Property;

namespace HomeHunt.ViewModels
{
    public class PropertyFormViewModel
    {
        private string _title = string.Empty;
        private string _description = string.Empty;
        private string? _imageUrl;

        public int Id { get; set; }

        [Required(ErrorMessage = TitleRequiredMessage)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength,
            ErrorMessage = TitleLengthMessage)]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = Helpers.Normalize(value);
            }
        }

        [Required(ErrorMessage = DescriptionRequiredMessage)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength,
            ErrorMessage = DescriptionLengthMessage)]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value == null ? string.Empty : value.Trim();
            }
        }

        [Required(ErrorMessage = PriceRequiredMessage)]
        [Range(PriceMinValue, PriceMaxValue, ErrorMessage = PriceRangeMessage)]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = AreaRequiredMessage)]
        [Range(AreaMinValue, AreaMaxValue, ErrorMessage = AreaRangeMessage)]
        [Display(Name = "Area (m²)")]
        public decimal? Area { get; set; }

        [Range(BedroomsMinValue, BedroomsMaxValue, ErrorMessage = BedroomsRangeMessage)]
        public int Bedrooms { get; set; }

        [Range(BathroomsMinValue, BathroomsMaxValue, ErrorMessage = BathroomsRangeMessage)]
        public int Bathrooms { get; set; }

        [EnumDataType(typeof(PropertyType), ErrorMessage = PropertyTypeInvalidMessage)]
        [Display(Name = "Property type")]
        public PropertyType PropertyType { get; set; }

        [EnumDataType(typeof(ListingType), ErrorMessage = ListingTypeInvalidMessage)]
        [Display(Name = "Listing type")]
        public ListingType ListingType { get; set; }

        [EnumDataType(typeof(PropertyStatus), ErrorMessage = StatusInvalidMessage)]
        public PropertyStatus Status { get; set; } = PropertyStatus.Available;

        [StringLength(ImageUrlMaxLength, ErrorMessage = ImageUrlMaxLengthMessage)]
        [RegularExpression(ImageUrlRegex, ErrorMessage = ImageUrlFormatMessage)]
        [Display(Name = "Image link (optional)")]
        public string? ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                _imageUrl = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        [Required(ErrorMessage = LocationRequiredMessage)]
        [Display(Name = "Location")]
        public int? LocationId { get; set; }

        [Required(ErrorMessage = AgentRequiredMessage)]
        [Display(Name = "Agent")]
        public int? AgentId { get; set; }

        public List<SelectListItem> Locations { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> Agents { get; set; } = new List<SelectListItem>();
    }
}
