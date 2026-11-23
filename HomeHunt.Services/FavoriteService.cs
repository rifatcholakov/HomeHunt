using HomeHunt.Data;
using HomeHunt.Data.Models;
using HomeHunt.Service.Data.Favorites;
using HomeHunt.Service.Data.Properties.Response;
using Microsoft.EntityFrameworkCore;

namespace HomeHunt.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ApplicationDbContext _context;

        public FavoriteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFavoriteAsync(string userId, int propertyId)
        {
            bool propertyExists = await _context.Properties.AnyAsync(p => p.Id == propertyId);

            if (!propertyExists)
            {
                return false;
            }

            bool alreadyFavorite = await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.PropertyId == propertyId);

            if (alreadyFavorite)
            {
                return true;
            }

            _context.Favorites.Add(new FavoriteProperty
            {
                UserId = userId,
                PropertyId = propertyId,
                CreatedOn = DateTime.Now
            });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return true;
            }

            return true;
        }

        public async Task RemoveFavoriteAsync(string userId, int propertyId)
        {
            FavoriteProperty? favorite = await _context.Favorites.FindAsync(userId, propertyId);

            if (favorite == null)
            {
                return;
            }

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task<HashSet<int>> GetFavoritePropertyIdsAsync(string userId)
        {
            List<int> ids = await _context.Favorites
                .AsNoTracking()
                .Where(f => f.UserId == userId)
                .Select(f => f.PropertyId)
                .ToListAsync();

            return ids.ToHashSet();
        }

        public async Task<IEnumerable<GetPropertiesResponseData>> GetFavoritePropertiesAsync(string userId)
        {
            return await _context.Favorites
                .AsNoTracking()
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.CreatedOn)
                .Select(f => new GetPropertiesResponseData
                {
                    Id = f.Property.Id,
                    Title = f.Property.Title,
                    City = f.Property.Location.City,
                    Neighborhood = f.Property.Location.Neighborhood,
                    Price = f.Property.Price,
                    Area = f.Property.Area,
                    Bedrooms = f.Property.Bedrooms,
                    Bathrooms = f.Property.Bathrooms,
                    PropertyType = f.Property.PropertyType,
                    ListingType = f.Property.ListingType,
                    Status = f.Property.Status,
                    ImageUrl = f.Property.ImageUrl,
                    OwnerUserId = f.Property.Agent.UserId
                })
                .ToListAsync();
        }
    }
}
