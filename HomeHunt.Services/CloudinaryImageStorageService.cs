using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HomeHunt.Common;
using HomeHunt.Service.Data.Images;
using HomeHunt.Service.Data.Images.Response;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HomeHunt.Services
{
    public class CloudinaryImageStorageService : IImageStorageService
    {
        private const string Folder = "homehunt/properties";
        private const string Host = "res.cloudinary.com";
        private const string UploadSegment = "/upload/";
        private const int DeleteBatchSize = 100;

        private readonly Cloudinary? _cloudinary;
        private readonly ILogger<CloudinaryImageStorageService> _logger;

        public CloudinaryImageStorageService(IOptions<CloudinaryOptions> options, ILogger<CloudinaryImageStorageService> logger)
        {
            _logger = logger;

            CloudinaryOptions settings = options.Value;

            if (settings.IsConfigured)
            {
                _cloudinary = new Cloudinary(new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret))
                {
                    Api = { Secure = true }
                };
            }
        }

        public async Task<UploadImageResponseData> UploadAsync(Stream content, string fileName, CancellationToken cancellationToken = default)
        {
            if (_cloudinary == null)
            {
                return new UploadImageResponseData { Error = Messages.Property.ImageStorageNotConfiguredMessage };
            }

            try
            {
                ImageUploadResult result = await _cloudinary.UploadAsync(new ImageUploadParams
                {
                    File = new FileDescription(fileName, content),
                    Folder = Folder,
                    UniqueFilename = true,
                    Overwrite = false
                }, cancellationToken);

                if (result.Error != null)
                {
                    _logger.LogWarning("Image upload was rejected: {Error}", result.Error.Message);
                    return new UploadImageResponseData { Error = Messages.Property.ImageUploadFailedMessage };
                }

                return new UploadImageResponseData
                {
                    Succeeded = true,
                    Url = result.SecureUrl.ToString(),
                    PublicId = result.PublicId
                };
            }
            catch (Exception exception) when (!cancellationToken.IsCancellationRequested
                && exception is HttpRequestException or TaskCanceledException or IOException)
            {
                _logger.LogError(exception, "Image upload failed.");
                return new UploadImageResponseData { Error = Messages.Property.ImageUploadFailedMessage };
            }
        }

        public async Task DeleteAsync(IEnumerable<string> publicIds)
        {
            if (_cloudinary == null)
            {
                return;
            }

            foreach (string[] batch in publicIds.Distinct().Chunk(DeleteBatchSize))
            {
                try
                {
                    await _cloudinary.DeleteResourcesAsync(ResourceType.Image, batch);
                }
                catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or IOException)
                {
                    _logger.LogWarning(exception, "Could not delete {Count} images from the image storage.", batch.Length);
                }
            }
        }

        public string GetResizedUrl(string url, int width, int height)
        {
            return Transform(url, $"c_fill,w_{width},h_{height},q_auto,f_auto");
        }

        public string GetFittedUrl(string url, int maxWidth)
        {
            return Transform(url, $"c_limit,w_{maxWidth},q_auto,f_auto");
        }

        private static string Transform(string url, string transformation)
        {
            if (!url.Contains(Host) || !url.Contains(UploadSegment))
            {
                return url;
            }

            int index = url.IndexOf(UploadSegment, StringComparison.Ordinal) + UploadSegment.Length;

            return url.Insert(index, transformation + "/");
        }
    }
}
