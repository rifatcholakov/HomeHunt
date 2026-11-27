using HomeHunt.Service.Data.Users.Request;
using HomeHunt.Service.Data.Users.Response;

namespace HomeHunt.Service.Data.Users
{
    public interface IUserService
    {
        Task<PagedResponseData<GetUserResponseData>> GetUsersAsync(string? search, int page, int pageSize);

        Task<GetUserResponseData?> GetUserAsync(string id);

        Task<OperationResponseData> ChangeRoleAsync(ChangeUserRoleRequestData request);

        Task<OperationResponseData> DeleteUserAsync(string id);
    }
}
