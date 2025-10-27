using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.ProductService.Dtos;
using Store.Services.Services.SubcategoryService.Dtos;

namespace Store.Services.Services.ProductService
{
    public interface IProductService
    {

        Task<CommonResponse<ProductResultDto>> AddProductAsync(ProductCreateDto dto);
        Task<CommonResponse<IReadOnlyList<ProductResultDto>>> GetAllProductsAsync();
        Task<CommonResponse<ProductResultDto>> GetProductByIdAsync(int productId);
        Task<CommonResponse<ProductResultDto>> UpdateProductAsync(int productId, ProductUpdateDto dto);
        Task<CommonResponse<bool>> DeleteProductAsync(int productId);
        Task<CommonResponse<ProductResultDto>> UpdateProductWithNewDiscountAsync(int productId, int discountId, bool useExistingTransaction);
        Task<CommonResponse<bool>> UpdateProductsWithNewDiscountAsync(List<int> productIds, int discountId, bool useExistingTransaction);
        Task<CommonResponse<decimal>> GetPriceAfterDiscountAsync(int productId);
    }
}
