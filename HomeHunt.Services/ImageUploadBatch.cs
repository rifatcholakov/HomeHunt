using HomeHunt.Data.Models;

namespace HomeHunt.Services
{
    public sealed record ImageUploadBatch(List<PropertyImage> Images, string? Error);
}
