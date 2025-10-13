using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.IdentityEntities;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Helper.Email;
using Store.Services.Services.EmailService;
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
        private readonly ILogger<UserService> _logger;
        private readonly IEmailService _emailService;
        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config,
            ITokenService tokenService,
            ILogger<UserService> logger,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _config = config;
            _logger = logger;
            _emailService = emailService;
        }
        public async Task<CommonResponse<UserDto>> LoginAsync(LoginDto loginDto)
        {
            var response = new CommonResponse<UserDto>();

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed for {Email}: user not found", loginDto.Email);
                return response.Fail("401", "Invalid Credential");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Invalid password for {Email}", loginDto.Email);
                return response.Fail("401", "Invalid Credential");
            }

            //if (!await _userManager.IsEmailConfirmedAsync(user))
            //    return response.Fail("403", "Email not confirmed");

            _logger.LogInformation("User {Email} logged in successfully", loginDto.Email);
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
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("User registration failed: {Errors}", errors);
                return response.Fail("400", errors);
            }

            _logger.LogInformation("User {Email} registered successfully", registerDto.Email);
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
                Title = "Reset Your Password",
                Body = $@"
                        <h2>Reset Your Password</h2>
                        <p>Hi {user.UserName},</p>
                        <p>Click the button below to reset your password:</p>
                        <a href='{resetPasswordLink}' style='
                            display:inline-block;
                            padding:10px 20px;
                            color:white;
                            background-color:#007bff;
                            border-radius:5px;
                            text-decoration:none;'>
                            Reset Password
                        </a>
                        <p>If you didn’t request a password reset, please ignore this email.</p>",
                To = forgetPasswordDto.Email
            };

            // Send Email
            //EmailSetting.SendEmail(email);
            try
            {
                await _emailService.SendEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send reset password email to {Email}", forgetPasswordDto.Email);
                return response.Fail("500", "Failed to send email. Please try again later.");
            }

            _logger.LogInformation("Password reset email sent to {Email}", forgetPasswordDto.Email);
            return response.Success(true); 
        }
        public async Task<CommonResponse<bool>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var response = new CommonResponse<bool>();
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return response.Fail("400", "Invalid Credential");

            var decodedToken = Uri.UnescapeDataString(resetPasswordDto.Token); // ✅ Important

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Password reset failed for {Email}: {Errors}", resetPasswordDto.Email, errors);
                return response.Fail("400", errors);
            }
            return response.Success(true);
        }
    }
}
