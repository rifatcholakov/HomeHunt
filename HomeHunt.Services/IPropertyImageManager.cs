using HomeHunt.Data.Models;
using HomeHunt.Service.Data.Images.Request;
using HomeHunt.Service.Data.Properties.Request;

namespace HomeHunt.Services
{
    public interface IPropertyImageManager
    {
        Task<ImageUploadBatch> UploadAsync(IReadOnlyCollection<ImageUploadRequestData> files, CancellationToken cancellationToken = default);

        List<PropertyImage> Arrange(
            IEnumerable<PropertyImage> kept,
            IReadOnlyList<PropertyImage> uploaded,
            IReadOnlyList<ImageOrderRequestData> requestedOrder);

        Task DeleteFromStorageAsync(IEnumerable<PropertyImage> images);
    }
}
