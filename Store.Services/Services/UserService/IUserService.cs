using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.UserService.Dtos;

namespace Store.Services.Services.UserService
{
    public interface IUserService
    {
        Task<CommonResponse<UserDto>> LoginAsync(LoginDto loginDto);
        Task<CommonResponse<UserDto>> RegisterAsync(RegisterDto registerDto);
    }
}
