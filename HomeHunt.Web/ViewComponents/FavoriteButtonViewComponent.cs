using System.Security.Claims;
using HomeHunt.Service.Data.Favorites;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomeHunt.ViewComponents
{
    public class FavoriteButtonViewComponent : ViewComponent
    {
        private const string FavoriteIdsItemKey = "FavoritePropertyIds";

        private readonly IFavoriteService _favoriteService;

        public FavoriteButtonViewComponent(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int propertyId)
        {
            string? userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            FavoriteButtonViewModel model = new FavoriteButtonViewModel
            {
                PropertyId = propertyId,
                IsAuthenticated = userId != null,
                ReturnUrl = Request.Path + Request.QueryString
            };

            if (userId != null)
            {
                HashSet<int> favoriteIds = await GetFavoriteIdsAsync(userId);
                model.IsFavorite = favoriteIds.Contains(propertyId);
            }

            return View(model);
        }

        private async Task<HashSet<int>> GetFavoriteIdsAsync(string userId)
        {
            if (HttpContext.Items[FavoriteIdsItemKey] is HashSet<int> cached)
            {
                return cached;
            }

            HashSet<int> favoriteIds = await _favoriteService.GetFavoritePropertyIdsAsync(userId);
            HttpContext.Items[FavoriteIdsItemKey] = favoriteIds;

            return favoriteIds;
        }
    }
}
