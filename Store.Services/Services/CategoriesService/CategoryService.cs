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
                categories = await _unitOfWork.Repository<Category, int>().GetAllAsync();
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
            Category category;
            var response = new CommonResponse<CategoryResultDto>();
            if (categoryId <= 0)
                return response.Fail("400", "Invalid Data, CategoryId must be greater than 0");
            try
            {
                category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(categoryId);
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
                category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(categoryId);
                if(category == null)
                    return response.Fail("404", "Category Not Found");

                if(!string.IsNullOrEmpty(dto.Name))
                    category.Name = dto.Name;
                if(!string.IsNullOrEmpty(dto.Description))
                    category.Description = dto.Description;

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
    }
}
