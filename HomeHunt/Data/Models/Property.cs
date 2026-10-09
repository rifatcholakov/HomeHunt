using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HomeHunt.Data.Models.Enums;
using static HomeHunt.Common.ValidationConstants.Property;

namespace HomeHunt.Data.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = string.Empty;

        [Range(PriceMinValue, PriceMaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(AreaMinValue, AreaMaxValue)]
        [Column(TypeName = "decimal(9,2)")]
        public decimal Area { get; set; }

        [Range(BedroomsMinValue, BedroomsMaxValue)]
        public int Bedrooms { get; set; }

        [Range(BathroomsMinValue, BathroomsMaxValue)]
        public int Bathrooms { get; set; }

        public PropertyType PropertyType { get; set; }

        public ListingType ListingType { get; set; }

        public PropertyStatus Status { get; set; }

        [StringLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        [ForeignKey(nameof(Location))]
        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;

        [ForeignKey(nameof(Agent))]
        public int AgentId { get; set; }
        public Agent Agent { get; set; } = null!;
    }
}
