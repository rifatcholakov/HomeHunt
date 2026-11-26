namespace HomeHunt.ViewModels
{
    public class UserIndexViewModel : PagedViewModel<UserListViewModel>
    {
        public string? Search { get; set; }
    }
}
