using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.RoleService;
using Store.Services.Services.RoleService.Dtos;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(
            IRoleService roleService
            )
        {
            _roleService = roleService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateRole(RoleCreateDto dto)
        {
            var result = await _roleService.CreateRoleAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRolesAsync();
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpGet]
        [Route("{roleId}")]
        public async Task<IActionResult> GetRole(string roleId)
        {
            var result = await _roleService.GetRoleByIdAsync(roleId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpDelete]
        [Route("{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var result = await _roleService.DeleteRoleAsync(roleId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpPut]
        [Route("{roleId}")]
        public async Task<IActionResult> UpdateRole(string roleId, RoleUpdateDto role)
        {
            var result = await _roleService.UpdateRoleAsync(roleId, role);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
    }
}
