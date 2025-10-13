using Store.Data.Entities.IdentityEntities;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.RoleService.Dtos;

namespace Store.Services.Services.RoleService
{
    public interface IRoleService
    {
        Task<CommonResponse<RoleResultDto>> CreateRoleAsync(RoleCreateDto dto);
        Task<CommonResponse<RoleResultDto>> UpdateRoleAsync(string roleId, RoleUpdateDto dto);

        Task<CommonResponse<RoleResultDto>> GetRoleByIdAsync(string roleId);
        Task<CommonResponse<List<RoleResultDto>>> GetAllRolesAsync();
        Task<CommonResponse<bool>> DeleteRoleAsync(string roleId);
    }
}
