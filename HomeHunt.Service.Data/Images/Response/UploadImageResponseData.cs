namespace HomeHunt.Service.Data.Images.Response
{
    public class UploadImageResponseData
    {
        public bool Succeeded { get; set; }

        public string Url { get; set; } = string.Empty;

        public string PublicId { get; set; } = string.Empty;

        public string? Error { get; set; }
    }
}
