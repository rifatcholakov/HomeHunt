namespace HomeHunt.ViewModels
{
    public class PagedViewModel<T>
    {
        public List<T> Items { get; set; } = new List<T>();

        public PaginationViewModel Pagination { get; set; } = new PaginationViewModel();
    }
}
