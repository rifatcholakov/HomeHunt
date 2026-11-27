using HomeHunt.Data;
using HomeHunt.Data.Models;
using HomeHunt.Common.Enums;
using Microsoft.EntityFrameworkCore;
using HomeHunt.Service.Data.Properties;
using HomeHunt.Service.Data.Properties.Request;
using HomeHunt.Service.Data.Properties.Response;
using HomeHunt.Common;
using HomeHunt.Service.Data;
using static HomeHunt.Common.ValidationConstants.Property;

namespace HomeHunt.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPropertyImageManager _images;

        public PropertyService(ApplicationDbContext context, IPropertyImageManager images)
        {
            _context = context;
            _images = images;
        }

        public async Task<PagedResponseData<GetPropertiesResponseData>> SearchPropertiesAsync(SearchPropertiesRequestData request)
        {
            IQueryable<Property> query = _context.Properties.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.City))
            {
                query = query.Where(p => p.Location.City == request.City);
            }

            if (request.PropertyType.HasValue)
            {
                query = query.Where(p => p.PropertyType == request.PropertyType.Value);
            }

            if (request.ListingType.HasValue)
            {
                query = query.Where(p => p.ListingType == request.ListingType.Value);
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            if (request.MinBedrooms.HasValue)
            {
                query = query.Where(p => p.Bedrooms >= request.MinBedrooms.Value);
            }

            if (request.MinArea.HasValue)
            {
                query = query.Where(p => p.Area >= request.MinArea.Value);
            }

            IOrderedQueryable<Property> orderedQuery;

            if (request.SortBy == SortOptions.PriceAsc)
            {
                orderedQuery = query.OrderBy(p => p.Price);
            }
            else if (request.SortBy == SortOptions.PriceDesc)
            {
                orderedQuery = query.OrderByDescending(p => p.Price);
            }
            else if (request.SortBy == SortOptions.Area)
            {
                orderedQuery = query.OrderByDescending(p => p.Area);
            }
            else
            {
                orderedQuery = query.OrderByDescending(p => p.CreatedOn);
            }

            return await orderedQuery
                .ThenBy(p => p.Id)
                .Select(PropertyProjections.ListItem)
                .ToPagedAsync(request.Page, request.PageSize);
        }

        public async Task<IEnumerable<GetPropertiesResponseData>> GetRecentPropertiesAsync(int count)
        {
            return await _context.Properties
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedOn)
                .Take(count)
                .Select(PropertyProjections.ListItem)
                .ToListAsync();
        }

        public async Task<int> CountAvailablePropertiesAsync()
        {
            return await _context.Properties
                .CountAsync(p => p.Status == PropertyStatus.Available);
        }

        public async Task<int> CountAvailablePropertiesForSaleAsync()
        {
            return await _context.Properties
                .CountAsync(p => p.Status == PropertyStatus.Available && p.ListingType == ListingType.Sale);
        }

        public async Task<int> CountAvailablePropertiesForRentAsync()
        {
            return await _context.Properties
                .CountAsync(p => p.Status == PropertyStatus.Available && p.ListingType == ListingType.Rent);
        }

        public async Task<OperationResponseData> CreatePropertyAsync(SavePropertyRequestData request, CancellationToken cancellationToken = default)
        {
            OperationResponseData response = new OperationResponseData();

            if (request.NewImages.Count > MaxImagesPerProperty)
            {
                response.Errors.Add(TooManyImagesMessage());
                return response;
            }

            Property property = new Property { CreatedOn = DateTime.Now };
            Apply(property, request);

            ImageUploadBatch uploaded = await _images.UploadAsync(request.NewImages, cancellationToken);

            if (uploaded.Error != null)
            {
                await _images.DeleteFromStorageAsync(uploaded.Images);
                response.Errors.Add(uploaded.Error);
                return response;
            }

            foreach (PropertyImage image in _images.Arrange(new List<PropertyImage>(), uploaded.Images, request.ImageOrder))
            {
                property.Images.Add(image);
            }

            _context.Properties.Add(property);

            await SaveOrRollBackUploadsAsync(uploaded.Images, cancellationToken);

            response.Succeeded = true;
            return response;
        }

        public async Task<GetPropertyForEditResponseData?> GetPropertyForEditAsync(int id)
        {
            return await _context.Properties
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new GetPropertyForEditResponseData
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    Area = p.Area,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    PropertyType = p.PropertyType,
                    ListingType = p.ListingType,
                    Status = p.Status,
                    LocationId = p.LocationId,
                    AgentId = p.AgentId,
                    Images = p.Images
                        .OrderBy(i => i.SortOrder)
                        .ThenBy(i => i.Id)
                        .Select(i => new GetPropertyImageResponseData { Id = i.Id, Url = i.Url })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<OperationResponseData> UpdatePropertyAsync(int id, UpdatePropertyRequestData request, CancellationToken cancellationToken = default)
        {
            OperationResponseData response = new OperationResponseData();

            Property? property = await _context.Properties
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (property == null)
            {
                response.NotFound = true;
                return response;
            }

            List<PropertyImage> removed = property.Images
                .Where(image => request.RemoveImageIds.Contains(image.Id))
                .ToList();

            List<PropertyImage> kept = property.Images.Except(removed).ToList();

            if (kept.Count + request.NewImages.Count > MaxImagesPerProperty)
            {
                response.Errors.Add(TooManyImagesMessage());
                return response;
            }

            ImageUploadBatch uploaded = await _images.UploadAsync(request.NewImages, cancellationToken);

            if (uploaded.Error != null)
            {
                await _images.DeleteFromStorageAsync(uploaded.Images);
                response.Errors.Add(uploaded.Error);
                return response;
            }

            Apply(property, request);

            _context.PropertyImages.RemoveRange(removed);

            foreach (PropertyImage image in _images.Arrange(kept, uploaded.Images, request.ImageOrder).Except(kept))
            {
                property.Images.Add(image);
            }

            await SaveOrRollBackUploadsAsync(uploaded.Images, cancellationToken);
            await _images.DeleteFromStorageAsync(removed);

            response.Succeeded = true;
            return response;
        }

        public async Task<bool> DeletePropertyAsync(int id, CancellationToken cancellationToken = default)
        {
            Property? property = await _context.Properties
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (property == null)
            {
                return false;
            }

            List<PropertyImage> images = property.Images.ToList();

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync(cancellationToken);
            await _images.DeleteFromStorageAsync(images);

            return true;
        }

        public async Task<string?> GetOwnerUserIdAsync(int propertyId)
        {
            return await _context.Properties
                .AsNoTracking()
                .Where(p => p.Id == propertyId)
                .Select(p => p.Agent.UserId)
                .FirstOrDefaultAsync();
        }

        public async Task<GetPropertyDetailsResponseData?> GetPropertyDetailsAsync(int id)
        {
            return await _context.Properties
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new GetPropertyDetailsResponseData
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    Area = p.Area,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    PropertyType = p.PropertyType,
                    ListingType = p.ListingType,
                    Status = p.Status,
                    CreatedOn = p.CreatedOn,
                    LocationId = p.LocationId,
                    City = p.Location.City,
                    Neighborhood = p.Location.Neighborhood,
                    AgentId = p.AgentId,
                    OwnerUserId = p.Agent.UserId,
                    AgentFirstName = p.Agent.FirstName,
                    AgentLastName = p.Agent.LastName,
                    AgentEmail = p.Agent.User.Email!,
                    AgentPhone = p.Agent.Phone,
                    Images = p.Images
                        .OrderBy(i => i.SortOrder)
                        .ThenBy(i => i.Id)
                        .Select(i => new GetPropertyImageResponseData { Id = i.Id, Url = i.Url })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        private static void Apply(Property property, SavePropertyRequestData request)
        {
            property.Title = request.Title;
            property.Description = request.Description;
            property.Price = request.Price;
            property.Area = request.Area;
            property.Bedrooms = request.Bedrooms;
            property.Bathrooms = request.Bathrooms;
            property.PropertyType = request.PropertyType;
            property.ListingType = request.ListingType;
            property.Status = request.Status;
            property.LocationId = request.LocationId;
            property.AgentId = request.AgentId;
        }

        private static string TooManyImagesMessage()
        {
            return string.Format(Messages.Property.ImagesTooManyMessage, MaxImagesPerProperty);
        }

        private async Task SaveOrRollBackUploadsAsync(List<PropertyImage> uploaded, CancellationToken cancellationToken)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                await _images.DeleteFromStorageAsync(uploaded);
                throw;
            }
        }
    }
}
