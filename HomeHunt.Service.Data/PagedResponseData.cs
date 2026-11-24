namespace HomeHunt.Service.Data
{
    public class PagedResponseData<T>
    {
        public List<T> Items { get; set; } = new List<T>();

        public int TotalCount { get; set; }

        public int Page { get; set; }

        public int TotalPages { get; set; }
    }
}
