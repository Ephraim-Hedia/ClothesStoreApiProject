using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Store.Repositories.Interfaces;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.DeliveryService.Dtos;

namespace Store.Services.Services.DeliveryService
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeliveryService> _logger;
        public DeliveryService(
            IUnitOfWork unitOfWork,
            ILogger<DeliveryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<CommonResponse<DeliveryResultDto>> GetDeliveryPriceByCityIdAsync(int cityId)
        {
            var response = new CommonResponse<DeliveryResultDto>();
            if (cityId <= 0)
                return response.Fail("400", "Invalid Data, City Id must be more than 0");
            try
            {
                return response;
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }

        }
    }
}
