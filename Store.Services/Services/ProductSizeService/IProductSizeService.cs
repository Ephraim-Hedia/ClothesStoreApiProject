using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.ProductSizeService.Dtos;

namespace Store.Services.Services.ProductSizeService
{
    public interface IProductSizeService
    {
        Task<CommonResponse<SizeResultDto>> AddSizeAsync(SizeCreateDto dto);
        Task<CommonResponse<bool>> DeleteSizeAsync(int sizeId);
        Task<CommonResponse<SizeResultDto>> GetSizeByIdAsync(int sizeId);
        Task<CommonResponse<IReadOnlyList<SizeResultDto>>> GetAllSizeAsync();
        Task<CommonResponse<SizeResultDto>> UpdateSizeAsync(int sizeId, SizeUpdateDto dto);

    }
}
