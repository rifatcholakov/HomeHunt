using HomeHunt.Service.Data.Users.Request;
using HomeHunt.Service.Data.Users.Response;

namespace HomeHunt.Service.Data.Users
{
    public interface IUserService
    {
        Task<PagedResponseData<GetUserResponseData>> GetUsersAsync(string? search, int page, int pageSize);

        Task<GetUserResponseData?> GetUserAsync(string id);

        Task<UserOperationResponseData> ChangeRoleAsync(ChangeUserRoleRequestData request);

        Task<UserOperationResponseData> DeleteUserAsync(string id);
    }
}
