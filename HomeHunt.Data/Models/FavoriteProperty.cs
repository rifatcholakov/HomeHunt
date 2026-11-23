using Microsoft.AspNetCore.Identity;

namespace HomeHunt.Data.Models
{
    public class FavoriteProperty
    {
        public string UserId { get; set; } = string.Empty;

        public IdentityUser User { get; set; } = null!;

        public int PropertyId { get; set; }

        public Property Property { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
    }
}
