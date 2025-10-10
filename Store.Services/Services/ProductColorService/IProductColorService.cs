using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.ProductColorService.Dtos;

namespace Store.Services.Services.ProductColorService
{
    public interface IProductColorService
    {
        Task<CommonResponse<ColorResultDto>> AddColorAsync(ColorCreateDto dto);
        Task<CommonResponse<bool>> DeleteColorAsync(int colorId);
        Task<CommonResponse<ColorResultDto>> UpdateColorAsync(int colorId, ColorUpdateDto dto);
        Task<CommonResponse<ColorResultDto>> GetColorByIdAsync(int colorId);
        Task<CommonResponse<IReadOnlyList<ColorResultDto>>> GetAllColorAsync();

    }
}
