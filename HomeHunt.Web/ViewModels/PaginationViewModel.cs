namespace HomeHunt.ViewModels
{
    public class PaginationViewModel
    {
        private const int PagesAroundCurrent = 2;

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<int> VisiblePages => Enumerable
            .Range(1, TotalPages)
            .Where(page => page == 1
                || page == TotalPages
                || Math.Abs(page - CurrentPage) <= PagesAroundCurrent);
    }
}
