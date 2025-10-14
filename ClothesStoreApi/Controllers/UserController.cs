using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.UserService;
using Store.Services.Services.UserService.Dtos;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(
            IUserService userService
            )
        {
            _userService = userService;
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateRole(UserCreateDto dto)
        {
            var result = await _userService.CreateUserAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _userService.GetAllUsersAsync();
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetRole(string userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> DeleteRole(string userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> UpdateRole(string userId, UserUpdateDto user)
        {
            var result = await _userService.UpdateUserAsync(userId, user);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
    }
}
