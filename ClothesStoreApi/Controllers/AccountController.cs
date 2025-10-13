using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.UserService;
using Store.Services.Services.UserService.Dtos;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(
            IUserService userService
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

    }
}
