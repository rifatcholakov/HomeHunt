using HomeHunt.Service.Data.Images.Response;

namespace HomeHunt.Service.Data.Images
{
    public interface IImageStorageService
    {
        Task<UploadImageResponseData> UploadAsync(Stream content, string fileName, CancellationToken cancellationToken = default);

        Task DeleteAsync(IEnumerable<string> publicIds);

        string GetResizedUrl(string url, int width, int height);

        string GetFittedUrl(string url, int maxWidth);
    }
}
