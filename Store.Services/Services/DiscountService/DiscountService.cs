using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.ProductEntities;
using Store.Repositories.Interfaces;
using Store.Repositories.Specification.ProductSpecification.DiscountSpecs;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.CategoriesService;
using Store.Services.Services.DiscountService.Dtos;

namespace Store.Services.Services.DiscountService
{
    public class DiscountService : IDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICategoryService _categoryService;
        public DiscountService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<DiscountService> logger,
            ICategoryService categoryService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _categoryService = categoryService;

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
                
                // Add the Discount First 
                await _unitOfWork.Repository<Discount , int>().AddAsync(discount);
                await _unitOfWork.CompleteAsync();

                // Apply Discount On Categories
                var result = await _categoryService.UpdateCategoriesWithNewDiscountAsync(dto.CategoryIds, discount.Id);
                if (!result.IsSuccess)
                {
                    response.Errors.Code = result.Errors.Code;
                    response.Errors.Message = "The Discount is Added But Error while Apply Discount On Categories : " + result.Errors.Message;
                    return response;
                }
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
                var specs = new DiscountWithRelationsSpecification();
                var discounts = await _unitOfWork.Repository<Discount, int>().GetAllWithSpecificationAsync(specs);
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
                var specs = new DiscountSpecificationWithId(discountId);
                var discount = await _unitOfWork.Repository<Discount, int>().GetByIdWithSpecificationAsync(specs);
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
            if ((dto.Percentage <= 0 || dto.Percentage > 100) && dto.Percentage != null)
                return response.Fail("400", "Invalid Data, Percentage must be between 0 to 100");

            try
            {
                // Check if the discount not exist 
                var discount = await _unitOfWork.Repository<Discount, int>().GetByIdAsync(discountId);
                if (discount == null)
                    return response.Fail("400", "Not Found discount Id");

                // Check if there is any update in the name of the discount
                if (!string.IsNullOrEmpty(dto.Name))
                {
                    
                    var specs = new DiscountSpecification(dto.Name);
                    var isExistingName = await _unitOfWork.Repository<Discount, int>().GetByIdWithSpecificationAsync(specs);
                    if (isExistingName == null)
                        discount.Name = dto.Name;
                    else
                        return response.Fail("400", "Invalid Data,Discount Name is Already Exist");
                }

                // Check if there is any update in the percentage of the discount
                if (dto.Percentage != null)
                    discount.Percentage = dto.Percentage.Value;

                // update the discount
                _unitOfWork.Repository<Discount, int>().Update(discount);
                await _unitOfWork.CompleteAsync();


                // The discount is updated but we want to check if 
                // there is any update we need to apply to relationship between the discount
                // and the categories, Subcategories or Products 
                if (dto.CategoryIds != null || dto.CategoryIds.Any())
                {
                    // Apply Discount On Categories
                    var result = await _categoryService.UpdateCategoriesWithNewDiscountAsync(dto.CategoryIds, discount.Id);
                    if (!result.IsSuccess)
                    {
                        response.Errors.Code = result.Errors.Code;
                        response.Errors.Message = "The Discount is Updated But Error while Apply Discount On Categories : " + result.Errors.Message;
                        return response;
                    }
                }
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
