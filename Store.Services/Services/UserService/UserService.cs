using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities.IdentityEntities;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Helper.Email;
using Store.Services.Services.TokenService;
using Store.Services.Services.UserService.Dtos;




namespace Store.Services.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _config = config;

        }
        public async Task<CommonResponse<UserDto>> LoginAsync(LoginDto loginDto)
        {
            var response = new CommonResponse<UserDto>();

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return response.Fail("401", "Invalid Credential");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return response.Fail("401", "Invalid Credential");


            var userDto = new UserDto() { Email = user.Email, Token = _tokenService.GenerateToken(user) };
            return response.Success(userDto);
        }

        public async Task<CommonResponse<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            var response = new CommonResponse<UserDto>();
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user is not null)
                return response.Fail("400", "User is already Exist");

            var appUser = new ApplicationUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0]
            };
            var result = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (!result.Succeeded)
                return response.Fail("400", "Error In Registering");

            var userDto =  new UserDto
            {
                Email = appUser.Email,
                Token = _tokenService.GenerateToken(appUser)
            };
            return response.Success(userDto);
        }

        public async Task<CommonResponse<bool>> SignOut()
        {
            var response = new CommonResponse<bool>();
            await _signInManager.SignOutAsync();
            return response.Success(true);
        }

        public async Task<CommonResponse<bool>> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            var response = new CommonResponse<bool>();
            var user = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);
            if (user == null)
                return response.Fail("400", "Invalid Credential");

            // Generate Password Reset Token
            // Create the link of reset Password 
            // Create an email 
            // Send Email 
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var baseUrl = _config["BaseUrl"];
            var resetPasswordLink = $"{baseUrl}/Account/ResetPassword?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token)}";
            var email = new TempEmail
            {
                Title = "Reset Password",
                Body = resetPasswordLink,
                To = forgetPasswordDto.Email
            };

            // Send Email
            EmailSetting.SendEmail(email);
            return response.Success(true); 
        }
        public async Task<CommonResponse<bool>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var response = new CommonResponse<bool>();
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return response.Fail("400", "Invalid Credential");

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!result.Succeeded)
                return response.Fail("400", "Error While Reset Password");

            return response.Success(true);
        }
    }
}
