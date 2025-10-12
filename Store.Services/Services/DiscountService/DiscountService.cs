using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.ProductEntities;
using Store.Repositories.Interfaces;
using Store.Repositories.Specification.ProductSpecification.DiscountSpecs;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.DiscountService.Dtos;

namespace Store.Services.Services.DiscountService
{
    public class DiscountService : IDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public DiscountService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<DiscountService> logger
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CommonResponse<DiscountResultDto>> AddDiscountAsync(DiscountCreateDto dto)
        {
            var response = new CommonResponse<DiscountResultDto>();
            Discount discount = new Discount();
            if (dto == null)
                return response.Fail("400", "Invalid Data, SubCategory Data is Null");
            if (string.IsNullOrEmpty(dto.Name))
                return response.Fail("400", "Invalid Data, SubCategory Name is Required");
            if (dto.Percentage <= 0)
                return response.Fail("400", "Invalid Data, CategoryId must be greater than 0");

            try
            {
                var specs = new DiscountSpecification(dto.Name);
                var isExistingName = await _unitOfWork.Repository<Discount, int>().GetByIdWithSpecificationAsync(specs);
                if (isExistingName != null)
                    return response.Fail("400", "Invalid Data, Discount Name Is already Exist");

                discount.Name = dto.Name;
                discount.Percentage = dto.Percentage;
                
                await _unitOfWork.Repository<Discount , int>().AddAsync(discount);
                await _unitOfWork.CompleteAsync();
                var mappedDiscount = _mapper.Map<DiscountResultDto>(discount);
                return response.Success(mappedDiscount);

            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
   
        }

        public async Task<CommonResponse<bool>> DeleteDiscountAsync(int discountId)
        {
            var response = new CommonResponse<bool>();  
            if(discountId <= 0 )
                return response.Fail("400", "Invalid Data, Discount Id must be greater than 0");

            try
            {
                var discount = await _unitOfWork.Repository<Discount, int>().GetByIdAsync(discountId);
                if (discount == null)
                    return response.Fail("404", "Discount Not Found");

                _unitOfWork.Repository<Discount, int>().Delete(discount);
                await _unitOfWork.CompleteAsync();
                return response.Success(true);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<IReadOnlyList<DiscountResultDto>>> GetAllDiscountAsync()
        {
            var response = new CommonResponse<IReadOnlyList<DiscountResultDto>>();
            try
            {
                var discounts = await _unitOfWork.Repository<Discount, int>().GetAllAsync();
                if (!discounts.Any())
                    return response.Fail("404", "No Discounts Found");
                var mappedDiscounts = _mapper.Map<IReadOnlyList<DiscountResultDto>>(discounts);
                return response.Success(mappedDiscounts);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<DiscountResultDto>> GetDiscountByIdAsync(int discountId)
        {
            var response = new CommonResponse<DiscountResultDto>();
            if (discountId <= 0)
                return response.Fail("400", "Invalid Data, Discount Id must be greater than 0");
            try
            {
                var discount = await _unitOfWork.Repository<Discount, int>().GetByIdAsync(discountId);
                if (discount == null)
                    return response.Fail("404", "Discount Not Found");
                var mappedDiscount = _mapper.Map<DiscountResultDto>(discount);
                return response.Success(mappedDiscount);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<DiscountResultDto>> UpdateDiscountAsync(int discountId, DiscountUpdateDto dto)
        {
            var response = new CommonResponse<DiscountResultDto>();
            if (discountId <= 0)
                return response.Fail("400", "Invalid Data, discount id must be greater than 0");
            if (dto == null)
                return response.Fail("400", "Invalid Data, Discount data is Null");
            if ((dto.Percentage <= 0 || dto.Percentage > 0) && dto.Percentage != null)
                return response.Fail("400", "Invalid Data, Percentage must be between 0 to 100");
            try
            {
                var discount = await _unitOfWork.Repository<Discount, int>().GetByIdAsync(discountId);
                if (discount == null)
                    return response.Fail("400", "Not Found discount Id");

                if (!string.IsNullOrEmpty(dto.Name))
                {
                    
                    var specs = new DiscountSpecification(dto.Name);
                    var isExistingName = await _unitOfWork.Repository<Discount, int>().GetByIdWithSpecificationAsync(specs);
                    if (isExistingName == null)
                        discount.Name = dto.Name;
                    else
                        return response.Fail("400", "Invalid Data,Category Name is Already Exist");
                }
                if (dto.Percentage != null)
                    discount.Percentage = dto.Percentage.Value;

                _unitOfWork.Repository<Discount, int>().Update(discount);
                await _unitOfWork.CompleteAsync();

                var mappedDiscount = _mapper.Map<DiscountResultDto>(discount);
                return response.Success(mappedDiscount);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }
    }
}
