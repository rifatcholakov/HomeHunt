namespace HomeHunt.Service.Data.Images.Request
{
    public class ImageUploadRequestData
    {
        public string FileName { get; set; } = string.Empty;

        public Stream Content { get; set; } = Stream.Null;
    }
}
