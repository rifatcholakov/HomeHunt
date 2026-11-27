using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HomeHunt.Common.ValidationConstants.Property;

namespace HomeHunt.Data.Models
{
    public class PropertyImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ImageUrlMaxLength)]
        public string Url { get; set; } = string.Empty;

        [StringLength(ImagePublicIdMaxLength)]
        public string? PublicId { get; set; }

        public int SortOrder { get; set; }

        [ForeignKey(nameof(Property))]
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
    }
}
