namespace HomeHunt.ViewModels
{
    public class FavoriteButtonViewModel
    {
        public int PropertyId { get; set; }

        public bool IsAuthenticated { get; set; }

        public bool IsFavorite { get; set; }

        public string ReturnUrl { get; set; } = string.Empty;
    }
}
