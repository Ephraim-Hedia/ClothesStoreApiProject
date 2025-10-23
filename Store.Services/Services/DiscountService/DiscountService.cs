using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.ProductEntities;
using Store.Repositories.Interfaces;
using Store.Repositories.Specification.ProductSpecification.DiscountSpecs;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.CategoriesService;
using Store.Services.Services.DiscountService.Dtos;
using Store.Services.Services.ProductService;
using Store.Services.Services.SubcategoryService;

namespace Store.Services.Services.DiscountService
{
    public class DiscountService : IDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICategoryService _categoryService;
        private readonly ISubcategoryService _subcategoryService;
        private readonly IProductService _productService;

        public DiscountService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<DiscountService> logger,
            ICategoryService categoryService,
            ISubcategoryService subcategoryService,
            IProductService productService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;
            _productService = productService;

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

            await _unitOfWork.BeginTransactionAsync(); // Start transaction

            try
            {
                var specs = new DiscountSpecification(dto.Name);
                var isExistingName = await _unitOfWork.Repository<Discount, int>().GetByIdWithSpecificationAsync(specs);
                if (isExistingName != null)
                    return response.Fail("400", "Invalid Data, Discount Name Is already Exist");

                // Create discount but do not save yet
                discount.Name = dto.Name;
                discount.Percentage = dto.Percentage;
                // Add the Discount First 
                await _unitOfWork.Repository<Discount , int>().AddAsync(discount);
                await _unitOfWork.CompleteAsync();

                // Apply Discount On Categories
                var categoriesResult = await _categoryService.UpdateCategoriesWithNewDiscountAsync(dto.CategoryIds, discount.Id , useExistingTransaction: true);
                if (!categoriesResult.IsSuccess)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return response.Fail(categoriesResult.Errors.Code, "Error while applying discount to Categories: " + categoriesResult.Errors.Message); ;
                }
                // Apply Discount On Subcategories
                var subcategoriesResult = await _subcategoryService.UpdateSubcategoriesWithNewDiscountAsync(dto.SubcategoryIds, discount.Id, useExistingTransaction: true);
                if (!subcategoriesResult.IsSuccess)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return response.Fail(subcategoriesResult.Errors.Code, "Error while applying discount to Subcategories: " + subcategoriesResult.Errors.Message);
                }
                // Apply Discount On Subcategories
                var productsResult = await _productService.UpdateProductsWithNewDiscountAsync(dto.ProductIds, discount.Id, useExistingTransaction: true);
                if (!productsResult.IsSuccess)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return response.Fail(productsResult.Errors.Code, "Error while applying discount to Products: " + productsResult.Errors.Message);
                }
                // Everything succeeded — now save all
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                var mappedDiscount = _mapper.Map<DiscountResultDto>(discount);
                return response.Success(mappedDiscount);

            }
            catch (Exception err)
            {
                await _unitOfWork.RollbackTransactionAsync();
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

            await _unitOfWork.BeginTransactionAsync();
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
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return response.Fail("400", "Invalid Data,Discount Name is Already Exist");
                    }
                }

                // Check if there is any update in the percentage of the discount
                if (dto.Percentage != null)
                    discount.Percentage = dto.Percentage.Value;

                // update the discount
                _unitOfWork.Repository<Discount, int>().Update(discount);


                // The discount is updated but we want to check if 
                // there is any update we need to apply to relationship between the discount
                // and the categories, Subcategories or Products 
                if (dto.CategoryIds != null )
                {
                    // Apply Discount On Categories
                    var categoryResult = await _categoryService.UpdateCategoriesWithNewDiscountAsync(dto.CategoryIds, discount.Id, useExistingTransaction: true);
                    if (!categoryResult.IsSuccess)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return response.Fail(categoryResult.Errors.Code, "Error while applying discount to Categories: " + categoryResult.Errors.Message);
                    }
                }
                if (dto.SubcategoryIds != null )
                {
                    // Apply Discount On Categories
                    var subcategoryResult = await _subcategoryService.UpdateSubcategoriesWithNewDiscountAsync(dto.SubcategoryIds, discount.Id, useExistingTransaction: true);
                    if (!subcategoryResult.IsSuccess)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return response.Fail(subcategoryResult.Errors.Code, "Error while applying discount to Subcategories: " + subcategoryResult.Errors.Message);

                    }
                }

                // All succeeded
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                var mappedDiscount = _mapper.Map<DiscountResultDto>(discount);
                return response.Success(mappedDiscount);
            }
            catch (Exception err)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(err.Message);
                throw;
            }
        }
    }
}
