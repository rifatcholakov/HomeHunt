using System.ComponentModel.DataAnnotations;
using HomeHunt.Common;
using HomeHunt.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using static HomeHunt.Common.ValidationConstants.Property;

namespace HomeHunt.ViewModels
{
    public class PropertyFormViewModel
    {
        private string _title = string.Empty;
        private string _description = string.Empty;
        private string? _imageUrl;

        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength,
            ErrorMessage = "Title must be between {2} and {1} characters.")]
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

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength,
            ErrorMessage = "Description must be between {2} and {1} characters.")]
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

        [Required(ErrorMessage = "Price is required.")]
        [Range(PriceMinValue, PriceMaxValue, ErrorMessage = "Price must be between {1:N0} and {2:N0}.")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Area is required.")]
        [Range(AreaMinValue, AreaMaxValue, ErrorMessage = "Area must be between {1:N0} and {2:N0} m².")]
        [Display(Name = "Area (m²)")]
        public decimal? Area { get; set; }

        [Range(BedroomsMinValue, BedroomsMaxValue, ErrorMessage = "Bedrooms must be between {1} and {2}.")]
        public int Bedrooms { get; set; }

        [Range(BathroomsMinValue, BathroomsMaxValue, ErrorMessage = "Bathrooms must be between {1} and {2}.")]
        public int Bathrooms { get; set; }

        [EnumDataType(typeof(PropertyType), ErrorMessage = "Please select a valid property type.")]
        [Display(Name = "Property type")]
        public PropertyType PropertyType { get; set; }

        [EnumDataType(typeof(ListingType), ErrorMessage = "Please select a valid listing type.")]
        [Display(Name = "Listing type")]
        public ListingType ListingType { get; set; }

        [EnumDataType(typeof(PropertyStatus), ErrorMessage = "Please select a valid status.")]
        public PropertyStatus Status { get; set; } = PropertyStatus.Available;

        [StringLength(ImageUrlMaxLength, ErrorMessage = "The link can be at most {1} characters.")]
        [RegularExpression(@"^https?://\S+$", ErrorMessage = "Enter a valid link that starts with http:// or https://")]
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

        [Required(ErrorMessage = "Please select a location.")]
        [Display(Name = "Location")]
        public int? LocationId { get; set; }

        [Required(ErrorMessage = "Please select an agent.")]
        [Display(Name = "Agent")]
        public int? AgentId { get; set; }

        public List<SelectListItem> Locations { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> Agents { get; set; } = new List<SelectListItem>();
    }
}
