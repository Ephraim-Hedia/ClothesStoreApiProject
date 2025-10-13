using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.IdentityEntities;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.RoleService.Dtos;

namespace Store.Services.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<RoleService> _logger;
        private readonly IMapper _mapper;
        public RoleService(
            RoleManager<ApplicationRole> roleManager,
            ILogger<RoleService> logger,
            IMapper mapper
            )
        {
            _roleManager = roleManager;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<CommonResponse<RoleResultDto>> CreateRoleAsync(RoleCreateDto dto)
        {
            var response = new CommonResponse<RoleResultDto>();
            ApplicationRole role = new ApplicationRole();
            if (dto == null)
                return response.Fail("400", "Invalid Data, Role is null");
            if (string.IsNullOrEmpty(dto.Name))
                return response.Fail("400", "Invalid Data, Role Name is Required");

            try
            {
                role.Name = dto.Name;
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                    return response.Fail("400", "Failed In Creating Role");

                var mappedRole = _mapper.Map<RoleResultDto>(role);
                return response.Success(mappedRole);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<bool>> DeleteRoleAsync(string roleId)
        {
            var response = new CommonResponse<bool>();
            if (string.IsNullOrEmpty(roleId))
                return response.Fail("400", "Invalid Data, Role Id is null");

            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null)
                    return response.Fail("404", "Role not found");

                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        _logger.LogError(error.Description);
                    
                    return response.Fail("400", "Error While Deleting Role");
                }

                return response.Success(true);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<List<RoleResultDto>>> GetAllRolesAsync()
        {
            var response = new CommonResponse<List<RoleResultDto>>();

            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                if (!roles.Any())
                    return response.Fail("404", "Not Found Roles");

                var mappedRoles = _mapper.Map<List<RoleResultDto>>(roles);
                return response.Success(mappedRoles);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<RoleResultDto>> GetRoleByIdAsync(string roleId)
        {
            var response = new CommonResponse<RoleResultDto>();
            if (string.IsNullOrEmpty(roleId))
                return response.Fail("400", "Invalid Data, role Id is null");

            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null)
                    return response.Fail("404", "Role not found");

                var mappedRole = _mapper.Map<RoleResultDto>(role);  
                return response.Success(mappedRole);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<RoleResultDto>> UpdateRoleAsync(string roleId, RoleUpdateDto dto)
        {
            var response = new CommonResponse<RoleResultDto>();
            if (string.IsNullOrEmpty(roleId))
                return response.Fail("400", "Invalid Data, role Id is null");
            if (dto == null)
                return response.Fail("400", "Invalid Data, role is null");

            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null)
                    return response.Fail("404", "role Not Found");

                if (!string.IsNullOrEmpty(dto.Name))
                    role.Name = dto.Name;

                var result = await _roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        _logger.LogError(error.Description);
                    
                    return response.Fail("400", "Error While Update Role");
                }
                var mappedRole = _mapper.Map<RoleResultDto>(role);
                return response.Success(mappedRole);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }
    }
}
