using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.ProductEntities;
using Store.Repositories.Interfaces;
using Store.Repositories.Specification.ProductSpecification.CategorySpecs;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.CategoriesService.Dtos;

namespace Store.Services.Services.CategoriesService
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CategoryService> logger
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CommonResponse<CategoryResultDto>> CreateCategoryAsync(CategoryCreateDto dto)
        {
            Category category = new Category();
            var response = new CommonResponse<CategoryResultDto>();
            
            if(dto == null)
                return response.Fail("400", "Invalid Data, Category Data is Null");
            if(string.IsNullOrEmpty(dto.Name))
                return response.Fail("400", "Invalid Data, Category Name is Required");
            category.Name = dto.Name;
            if(!string.IsNullOrEmpty(dto.Description))
                category.Description = dto.Description;
            if(dto.DiscountId != null && dto.DiscountId <= 0)
                return response.Fail("400", "Invalid Data, DiscountId must be greater than 0");

            try
            {             
                // Make the Category Name Unique 
                var specs = new CategorySpecification(dto.Name);
                var categoryWithSameName = await _unitOfWork.Repository<Category, int>()
                    .GetByIdWithSpecificationAsync(specs);
                if(categoryWithSameName != null)
                    return response.Fail("400", "Invalid Data, Category Name is Already Exist");
                if(dto.DiscountId != null)
                {
                    var discount = await _unitOfWork.Repository<Discount, int>()
                        .GetByIdAsync(dto.DiscountId.Value);
                    if(discount == null)
                        return response.Fail("404", "Discount Not Found");
                    category.DiscountId = dto.DiscountId;
                }
                await _unitOfWork.Repository<Category, int>().AddAsync(category);
                await _unitOfWork.CompleteAsync();

                var mappedCategory = _mapper.Map<CategoryResultDto>(category);
                return response.Success(mappedCategory);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }          
        }

        public async Task<CommonResponse<bool>> DeleteCategoryAsync(int categoryId)
        {
            var response = new CommonResponse<bool>();

            if (categoryId <= 0)
                return response.Fail("400", "Invalid Data, CategoryId must be greater than 0");
            try
            {
                var category = await _unitOfWork.Repository<Category , int>().GetByIdAsync(categoryId);
                if(category == null)
                    return response.Fail("404", "Category Not Found");
                _unitOfWork.Repository<Category, int>().Delete(category);
                await _unitOfWork.CompleteAsync();
                return response.Success(true);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
            
        }

        public async Task<CommonResponse<IReadOnlyList<CategoryResultDto>>> GetAllCategoriesAsync()
        {
            IReadOnlyList<Category> categories;
            var response = new CommonResponse<IReadOnlyList<CategoryResultDto>>();
            try
            {
                var paramters = new CategorySpecsParameters();
                var specs = new CategorySpecification(paramters);
                categories = await _unitOfWork.Repository<Category, int>().GetAllWithSpecificationAsync(specs);
                if (!categories.Any())
                    return response.Fail("404", "No Categories Found");
                var mappedCategories = _mapper.Map<IReadOnlyList<CategoryResultDto>>(categories);
                return response.Success(mappedCategories);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
            
        }

        public async Task<CommonResponse<CategoryResultDto>> GetCategoryByIdAsync(int categoryId)
        {
            
            var response = new CommonResponse<CategoryResultDto>();
            if (categoryId <= 0)
                return response.Fail("400", "Invalid Data, CategoryId must be greater than 0");
            try
            {
                var specs = new CategorySpecificationWithId(categoryId);
                var category = await _unitOfWork.Repository<Category, int>().GetByIdWithSpecificationAsync(specs);
                if(category == null)
                    return response.Fail("404", "Category Not Found");
                var mappedCategory = _mapper.Map<CategoryResultDto>(category);
                return response.Success(mappedCategory);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }


        public async Task<CommonResponse<CategoryResultDto>> UpdateCategoryAsync(int categoryId, CategoryUpdateDto dto)
        {
            var response = new CommonResponse<CategoryResultDto>();
            Category category;
            if (categoryId <= 0)
                return response.Fail("400", "Invalid Data, CategoryId must be greater than 0");
            if(dto == null)
                return response.Fail("400", "Invalid Data,Category Data is Null");
            
            try
            {
                var specs = new CategorySpecificationWithId(categoryId);
                category = await _unitOfWork.Repository<Category, int>().GetByIdWithSpecificationAsync(specs);
                if(category == null)
                    return response.Fail("404", "Category Not Found");

                if(!string.IsNullOrEmpty(dto.Name))
                {
                    var NameSpecs = new CategorySpecification(dto.Name);
                    var isExistingName = await _unitOfWork.Repository<Category, int>().GetByIdWithSpecificationAsync(NameSpecs);
                    if(isExistingName == null)
                        category.Name = dto.Name;
                    else
                        return response.Fail("400", "Invalid Data,Category Name is Already Exist");
                }
                if(!string.IsNullOrEmpty(dto.Description))
                    category.Description = dto.Description;
                if (dto.DiscountId != null && dto.DiscountId > 0)
                {
                    var discount = await _unitOfWork.Repository<Discount, int>().GetByIdAsync(dto.DiscountId.Value);
                    if (discount == null)
                        return response.Fail("404", "Discount Not Found");

                    category.DiscountId = dto.DiscountId;
                }

                _unitOfWork.Repository<Category, int>().Update(category);
                await _unitOfWork.CompleteAsync();

                var mappedCategory = _mapper.Map<CategoryResultDto>(category);
                return response.Success(mappedCategory);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }
        public async Task<CommonResponse<bool>> UpdateCategoriesWithNewDiscountAsync(
            List<int> categoryIds,
            int discountId,
            bool useExistingTransaction = false)
        {
            var response = new CommonResponse<bool>();
            if (discountId <= 0)
                return response.Fail("400", "Discount Id Must be more than 0");
            if (!categoryIds.Any() || categoryIds == null)
                return response.Fail("400", "Category Ids is null or empty");

            
            try
            {
                if (!useExistingTransaction)
                    await _unitOfWork.BeginTransactionAsync(); // only if not already in one

                var discount = await _unitOfWork.Repository<Discount, int>().GetByIdAsync(discountId);
                if (discount == null)
                    return response.Fail("404", "Not found Discount");
                foreach (var categoryId in categoryIds)
                {
                    var result = await UpdateCategoryWithNewDiscountAsync(categoryId, discountId , true);
                    if (!result.IsSuccess)
                    {
                        response.Errors.Code = result.Errors.Code;
                        response.Errors.Message = result.Errors.Message;
                        return response;
                    }
                }
                await _unitOfWork.CompleteAsync();

                if (!useExistingTransaction)
                    await _unitOfWork.CommitTransactionAsync();
                return response.Success(true);
            }
            catch (Exception err)
            {
                if (!useExistingTransaction)
                    await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<CategoryResultDto>> UpdateCategoryWithNewDiscountAsync(
            int categoryId,
            int discountId,
            bool useExistingTransaction = false)
        {
            var response = new CommonResponse<CategoryResultDto>();
            if (categoryId <= 0 || discountId <= 0)
                return response.Fail("400", "Invalid data, discount Id and Category Id must be more than 0");

            try
            {
                if (!useExistingTransaction)
                    await _unitOfWork.BeginTransactionAsync();

                var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(categoryId);
                if (category == null)
                    return response.Fail("404", "Category Not Found");

                var discount = await _unitOfWork.Repository<Discount, int>().GetByIdAsync(discountId);
                if (discount == null)
                    return response.Fail("404", "Discount Not Found");

                category.Discount = discount;
                _unitOfWork.Repository<Category, int>().Update(category);
                if (!useExistingTransaction)
                {
                    await _unitOfWork.CompleteAsync();
                    await _unitOfWork.CommitTransactionAsync();
                }
                var mappedCategory = _mapper.Map<CategoryResultDto>(category);

                return response.Success(mappedCategory);
            }
            catch (Exception err)
            {
                if (!useExistingTransaction)
                    await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(err.Message);
                throw;
            }

        }
    }
}
