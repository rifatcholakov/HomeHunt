namespace HomeHunt.Service.Data.Properties.Response
{
    public class SearchPropertiesResponseData
    {
        public List<GetPropertiesResponseData> Items { get; set; } = new List<GetPropertiesResponseData>();

        public int TotalCount { get; set; }

        public int Page { get; set; }

        public int TotalPages { get; set; }
    }
}
