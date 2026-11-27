namespace HomeHunt.Service.Data.Properties.Request
{
    public class UpdatePropertyRequestData : SavePropertyRequestData
    {
        public List<int> RemoveImageIds { get; set; } = new List<int>();
    }
}
