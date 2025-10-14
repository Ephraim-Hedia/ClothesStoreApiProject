using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.AccountService.Dtos;

namespace Store.Services.Services.AccountService
{
    public interface IAccountService
    {
        Task<CommonResponse<UserDto>> LoginAsync(LoginDto loginDto);
        Task<CommonResponse<UserDto>> RegisterAsync(RegisterDto registerDto);
        public Task<CommonResponse<bool>> SignOut();
        public Task<CommonResponse<bool>> ForgetPassword(ForgetPasswordDto forgetPasswordDto);
        public Task<CommonResponse<bool>> ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}
