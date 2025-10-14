using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.AccountService;
using Store.Services.Services.AccountService.Dtos;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _userService;
        public AccountController(
            IAccountService userService
            )
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto input)
        {
            var result = await _userService.LoginAsync(input);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDto input)
        {
            var result = await _userService.RegisterAsync(input);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPost]
        [Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            var result = await _userService.SignOut();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPost]
        [Route("forget-password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto input)
        {
            var result = await _userService.ForgetPassword(input);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto input)
        {
            var result = await _userService.ResetPassword(input);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

    }
}
