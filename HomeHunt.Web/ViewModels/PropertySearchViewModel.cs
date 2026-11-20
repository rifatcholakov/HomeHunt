using System.ComponentModel.DataAnnotations;
using HomeHunt.Common;
using HomeHunt.Common.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

        [Range(0, PriceMaxValue, ErrorMessage = MinPriceRangeMessage)]
        [Display(Name = "Min price")]
        public decimal? MinPrice { get; set; }

        [Range(0, PriceMaxValue, ErrorMessage = MaxPriceRangeMessage)]
        [Display(Name = "Max price")]
        public decimal? MaxPrice { get; set; }

        [Range(0, BedroomsMaxValue, ErrorMessage = MinBedroomsRangeMessage)]
        [Display(Name = "Min bedrooms")]
        public int? MinBedrooms { get; set; }

        [Range(0, AreaMaxValue, ErrorMessage = MinAreaRangeMessage)]
        [Display(Name = "Min area (m²)")]
        public decimal? MinArea { get; set; }

        [Display(Name = "Sort by")]
        public string SortBy { get; set; } = SortOptions.Newest;

        public List<string> Cities { get; set; } = new List<string>();

        public List<PropertyListViewModel> Properties { get; set; } = new List<PropertyListViewModel>();
    }
}
