using System.Security.Claims;
using HomeHunt.Service.Data.Favorites;
using HomeHunt.Service.Data.Properties.Response;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHunt.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<GetPropertiesResponseData> response = await _favoriteService.GetFavoritePropertiesAsync(GetUserId());

            List<PropertyListViewModel> model = response
                .Select(p => new PropertyListViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    City = p.City,
                    Neighborhood = p.Neighborhood,
                    Price = p.Price,
                    Area = p.Area,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    PropertyType = p.PropertyType,
                    ListingType = p.ListingType,
                    Status = p.Status,
                    ImageUrl = p.ImageUrl,
                    OwnerUserId = p.OwnerUserId
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int propertyId, string? returnUrl)
        {
            bool added = await _favoriteService.AddFavoriteAsync(GetUserId(), propertyId);

            if (!added)
            {
                return NotFound();
            }

            return RedirectBack(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int propertyId, string? returnUrl)
        {
            await _favoriteService.RemoveFavoriteAsync(GetUserId(), propertyId);

            return RedirectBack(returnUrl);
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

        private IActionResult RedirectBack(string? returnUrl)
        {
            if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            {
                return NoContent();
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
