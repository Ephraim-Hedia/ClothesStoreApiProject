using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.DeliveryService.Dtos;

namespace Store.Services.Services.DeliveryService
{
    public interface IDeliveryService
    {
        Task<CommonResponse<DeliveryResultDto>> GetDeliveryPriceByCityIdAsync(int cityId);

    }
}
