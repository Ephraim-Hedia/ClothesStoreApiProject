using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.UserService.Dtos;

namespace Store.Services.Services.UserService
{
    public interface IUserService
    {
        Task<CommonResponse<UserResultDto>> CreateUserAsync(UserCreateDto dto);
        Task<CommonResponse<UserResultDto>> UpdateUserAsync(string userId, UserUpdateDto dto);

        Task<CommonResponse<UserResultDto>> GetUserByIdAsync(string userId);
        Task<CommonResponse<IReadOnlyList<UserResultDto>>> GetAllUsersAsync();
        Task<CommonResponse<bool>> DeleteUserAsync(string userId);
    }
}
