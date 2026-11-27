using System.ComponentModel.DataAnnotations;
using HomeHunt.Common.Enums;
using HomeHunt.Common;
using static HomeHunt.Common.ValidationConstants.Property;
using static HomeHunt.Common.Messages.Property;

namespace HomeHunt.ViewModels
{
    public class PropertySearchViewModel
    {
        public string? City { get; set; }

        [Display(Name = "Property type")]
        public PropertyType? PropertyType { get; set; }

        [Display(Name = "Listing type")]
        public ListingType? ListingType { get; set; }

        [Range(SearchMinValue, PriceMaxValue, ErrorMessage = MinPriceRangeMessage)]
        [Display(Name = "Min price")]
        public decimal? MinPrice { get; set; }

        [Range(SearchMinValue, PriceMaxValue, ErrorMessage = MaxPriceRangeMessage)]
        [Display(Name = "Max price")]
        public decimal? MaxPrice { get; set; }

        [Range(SearchMinValue, BedroomsMaxValue, ErrorMessage = MinBedroomsRangeMessage)]
        [Display(Name = "Min bedrooms")]
        public int? MinBedrooms { get; set; }

        [Range(SearchMinValue, AreaMaxValue, ErrorMessage = MinAreaRangeMessage)]
        [Display(Name = "Min area (m²)")]
        public decimal? MinArea { get; set; }

        [Display(Name = "Sort by")]
        public string SortBy { get; set; } = SortOptions.Newest;

        public int Page { get; set; } = 1;

        public int TotalPages { get; set; } = 1;

        public int TotalCount { get; set; }

        public List<string> Cities { get; set; } = new List<string>();

        public List<PropertyListViewModel> Properties { get; set; } = new List<PropertyListViewModel>();
    }
}
