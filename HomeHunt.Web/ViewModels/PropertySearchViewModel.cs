using System.ComponentModel.DataAnnotations;
using HomeHunt.Common.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static HomeHunt.Common.ValidationConstants.Property;

namespace HomeHunt.ViewModels
{
    public class PropertySearchViewModel
    {
        public string? City { get; set; }

        [Display(Name = "Property type")]
        public PropertyType? PropertyType { get; set; }

        [Display(Name = "Listing type")]
        public ListingType? ListingType { get; set; }

        [Range(0, PriceMaxValue, ErrorMessage = "Minimum price must be between {1:N0} and {2:N0}.")]
        [Display(Name = "Min price")]
        public decimal? MinPrice { get; set; }

        [Range(0, PriceMaxValue, ErrorMessage = "Maximum price must be between {1:N0} and {2:N0}.")]
        [Display(Name = "Max price")]
        public decimal? MaxPrice { get; set; }

        [Range(0, BedroomsMaxValue, ErrorMessage = "Minimum bedrooms must be between {1} and {2}.")]
        [Display(Name = "Min bedrooms")]
        public int? MinBedrooms { get; set; }

        [Range(0, AreaMaxValue, ErrorMessage = "Minimum area must be between {1} and {2:N0}.")]
        [Display(Name = "Min area (m²)")]
        public decimal? MinArea { get; set; }

        [Display(Name = "Sort by")]
        public string SortBy { get; set; } = "newest";

        public List<string> Cities { get; set; } = new List<string>();

        public List<PropertyListViewModel> Properties { get; set; } = new List<PropertyListViewModel>();
    }
}
