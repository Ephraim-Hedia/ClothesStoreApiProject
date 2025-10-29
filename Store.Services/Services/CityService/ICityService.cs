using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.CityService.Dtos;

namespace Store.Services.Services.CityService
{
    public interface ICityService
    {
        Task<CommonResponse<CityResultDto>> AddCityAsync(CityCreateDto dto);
        Task<CommonResponse<bool>> DeleteCityAsync(int cityId);
        Task<CommonResponse<CityResultDto>> GetCityByIdAsync(int cityId);
        Task<CommonResponse<IReadOnlyList<CityResultDto>>> GetAllCityAsync();
        Task<CommonResponse<IReadOnlyList<CityDropDownListResultDto>>> GetAllCityDropListAsync();

        Task<CommonResponse<CityResultDto>> UpdateCityAsync(int cityId, CityUpdateDto dto);
    }
}
