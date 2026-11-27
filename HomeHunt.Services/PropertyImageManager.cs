using HomeHunt.Data.Models;
using HomeHunt.Service.Data.Images;
using HomeHunt.Service.Data.Images.Request;
using HomeHunt.Service.Data.Images.Response;
using HomeHunt.Service.Data.Properties.Request;
using HomeHunt.Common;

namespace HomeHunt.Services
{
    public class PropertyImageManager : IPropertyImageManager
    {
        private readonly IImageStorageService _storage;

        public PropertyImageManager(IImageStorageService storage)
        {
            _storage = storage;
        }

        public async Task<ImageUploadBatch> UploadAsync(IReadOnlyCollection<ImageUploadRequestData> files, CancellationToken cancellationToken = default)
        {
            Task<UploadImageResponseData>[] uploads = files
                .Select(file => _storage.UploadAsync(file.Content, file.FileName, cancellationToken))
                .ToArray();

            try
            {
                await Task.WhenAll(uploads);
            }
            catch
            {
                await DeleteFromStorageAsync(ToEntities(uploads));
                throw;
            }

            UploadImageResponseData? failed = uploads
                .Select(upload => upload.Result)
                .FirstOrDefault(result => !result.Succeeded);

            string? error = failed == null ? null : failed.Error ?? Messages.Property.ImageUploadFailedMessage;

            return new ImageUploadBatch(ToEntities(uploads), error);
        }

        public List<PropertyImage> Arrange(
            IEnumerable<PropertyImage> kept,
            IReadOnlyList<PropertyImage> uploaded,
            IReadOnlyList<ImageOrderRequestData> requestedOrder)
        {
            List<PropertyImage> existing = kept
                .OrderBy(image => image.SortOrder)
                .ThenBy(image => image.Id)
                .ToList();

            List<PropertyImage> ordered = new List<PropertyImage>();

            foreach (ImageOrderRequestData item in requestedOrder)
            {
                PropertyImage? image = item.ExistingImageId != null
                    ? existing.FirstOrDefault(i => i.Id == item.ExistingImageId)
                    : item.NewImageIndex >= 0 && item.NewImageIndex < uploaded.Count
                        ? uploaded[item.NewImageIndex.Value]
                        : null;

                if (image != null && !ordered.Contains(image))
                {
                    ordered.Add(image);
                }
            }

            ordered.AddRange(existing.Concat(uploaded).Where(image => !ordered.Contains(image)));

            for (int index = 0; index < ordered.Count; index++)
            {
                ordered[index].SortOrder = index;
            }

            return ordered;
        }

        public async Task DeleteFromStorageAsync(IEnumerable<PropertyImage> images)
        {
            List<string> publicIds = images
                .Where(image => image.PublicId != null)
                .Select(image => image.PublicId!)
                .ToList();

            if (publicIds.Count > 0)
            {
                await _storage.DeleteAsync(publicIds);
            }
        }

        private static List<PropertyImage> ToEntities(IEnumerable<Task<UploadImageResponseData>> uploads)
        {
            return uploads
                .Where(upload => upload.IsCompletedSuccessfully && upload.Result.Succeeded)
                .Select(upload => new PropertyImage { Url = upload.Result.Url, PublicId = upload.Result.PublicId })
                .ToList();
        }
    }
}
