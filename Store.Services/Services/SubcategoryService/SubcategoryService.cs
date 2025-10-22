using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.ProductEntities;
using Store.Repositories.Interfaces;
using Store.Repositories.Specification.ProductSpecification.SubcategorySpecs;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.SubcategoryService.Dtos;

namespace Store.Services.Services.SubcategoryService
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; 
        private readonly ILogger<SubcategoryService> _logger;
        public SubcategoryService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<SubcategoryService> logger
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CommonResponse<SubcategoryResultDto>> AddSubcategoryAsync(SubcategoryCreateDto dto)
        {
            var response = new CommonResponse<SubcategoryResultDto>();
            Subcategory subcategory = new Subcategory();
            if(dto == null)
                return response.Fail("400", "Invalid Data, SubCategory Data is Null");
            if(string.IsNullOrEmpty(dto.Name))
                return response.Fail("400", "Invalid Data, SubCategory Name is Required");
            if(dto.CategoryId <= 0)
                return response.Fail("400", "Invalid Data, CategoryId must be greater than 0");
            if(dto.DiscountId != null && dto.DiscountId <= 0)
                return response.Fail("400", "Invalid Data, DiscountId must be greater than 0");

            try
            {
                // check if Subcategory with the same name already exists
                var specs = new SubcategorySpecification(dto.Name);
                var existingSubcategoryWithSameName = await _unitOfWork.Repository<Subcategory, int>().GetByIdWithSpecificationAsync(specs);
                if(existingSubcategoryWithSameName != null)
                    return response.Fail("400", "Invalid Data, Subcategory Name is Already Exist");
                subcategory.Name = dto.Name;

                // Check if the discription exists
                if(!string.IsNullOrEmpty(dto.Description))
                    subcategory.Description = dto.Description;

                // Check if Discount Exists
                if (dto.DiscountId != null)
                {
                    var discount = await _unitOfWork.Repository<Discount, int>().GetByIdAsync(dto.DiscountId.Value);
                    if (discount == null)
                        return response.Fail("404", "Discount not found, cannot add Subcategory");
                    subcategory.DiscountId = dto.DiscountId;
                }

                // Check if Category Exists
                var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(dto.CategoryId);
                if (category == null)
                    return response.Fail("404", "Category not found, cannot add Subcategory");
                subcategory.CategoryId = category.Id;


                await _unitOfWork.Repository<Subcategory,int>().AddAsync(subcategory);
                await _unitOfWork.CompleteAsync();

                var mappedSubcategory = _mapper.Map<SubcategoryResultDto>(subcategory);
                return response.Success(mappedSubcategory);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<bool>> DeleteSubcategoryAsync(int subcategoryId)
        {
            var response = new CommonResponse<bool>();
            Subcategory subcategory;
            if (subcategoryId <= 0)
                return response.Fail("400", "Invalid Data, Subcategory Id must be greater than 0");
            try
            {
                subcategory = await _unitOfWork.Repository<Subcategory, int>().GetByIdAsync(subcategoryId);
                if(subcategory == null)
                    return response.Fail("404", "SubCategory Not Found");
                _unitOfWork.Repository<Subcategory, int>().Delete(subcategory);
                await _unitOfWork.CompleteAsync();
                return response.Success(true);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }  
        }

        public async Task<CommonResponse<IReadOnlyList<SubcategoryResultDto>>> GetAllSubcategoryAsync()
        {
            var response = new CommonResponse<IReadOnlyList<SubcategoryResultDto>>();
            IReadOnlyList<Subcategory> subcategories;
            try
            {
                var parameters = new SubcategorySpecsParameters();
                var specs = new SubcategorySpecification(parameters);
                subcategories = await _unitOfWork.Repository<Subcategory, int>().GetAllWithSpecificationAsync(specs);
                if(!subcategories.Any())
                    return response.Fail("404", "No SubCategories Found");
                var mappedSubcategories = _mapper.Map<IReadOnlyList<SubcategoryResultDto>>(subcategories);
                return response.Success(mappedSubcategories);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<SubcategoryResultDto>> GetSubcategoryByIdAsync(int subcategoryId)
        {
            var response = new CommonResponse<SubcategoryResultDto>();
        
            if (subcategoryId <= 0)
                return response.Fail("400", "Invalid Data, Subcategory Id must be greater than 0");
            try
            {
                var specs = new SubcategorySpecificationById(subcategoryId);
                var subcategory = await _unitOfWork.Repository<Subcategory, int>().GetByIdWithSpecificationAsync(specs);
                if (subcategory == null)
                    return response.Fail("404", "SubCategory Not Found");
                var mappedSubcategory = _mapper.Map<SubcategoryResultDto>(subcategory);
               return response.Success(mappedSubcategory);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
            
        }

        public async Task<CommonResponse<SubcategoryResultDto>> UpdateSubcategoryAsync(int subcategoryId, SubcategoryUpdateDto dto)
        {
            var response = new CommonResponse<SubcategoryResultDto>();
            Subcategory subcategory;
            if (subcategoryId <= 0)
                return response.Fail("400", "Invalid Data, Subcategory Id must be greater than 0");
            if (dto == null)
                return response.Fail("400", "Invalid Data, SubCategory Data is Null");

            if(dto.CategoryId != null && dto.CategoryId <= 0)
                return response.Fail("400", "Invalid Data, CategoryId must be greater than 0");
            if(dto.DiscountId != null && dto.DiscountId <= 0)
                return response.Fail("400", "Invalid Data, DiscountId must be greater than 0");
            

            try
            {
                // Get the Subcategory to be updated
                var specs = new SubcategorySpecificationById(subcategoryId);
                subcategory = await _unitOfWork.Repository<Subcategory, int>().GetByIdWithSpecificationAsync(specs);
                if (subcategory == null)
                    return response.Fail("404", "SubCategory Not Found");

                // Update the Subcategory properties
                // Only update the properties that are not null or empty in the dto
                
                if (!string.IsNullOrEmpty(dto.Name))
                {
                    var Namespecs = new SubcategorySpecification(dto.Name);
                    var isExistingName = await _unitOfWork.Repository<Subcategory, int>().GetByIdWithSpecificationAsync(Namespecs);
                    if (isExistingName != null)
                        return response.Fail("400", "Invalid Data, Subcategory Name is already Exist");
                    subcategory.Name = dto.Name;
                }
                if (!string.IsNullOrEmpty(dto.Description))
                    subcategory.Description = dto.Description;


                if(dto.CategoryId != null)
                {
                    var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(dto.CategoryId.Value);
                    if (category == null)
                        return response.Fail("400", "Category Id not found, cannot update Subcategory");
                    subcategory.CategoryId = category.Id;
                }

                if (dto.DiscountId != null)
                {
                    var discount = await _unitOfWork.Repository<Discount, int>().GetByIdAsync(dto.DiscountId.Value);
                    if (discount == null)
                        return response.Fail("400", "Discount not found, cannot update Subcategory");
                    subcategory.DiscountId = discount.Id;
                }
                
                _unitOfWork.Repository<Subcategory, int>().Update(subcategory);
                await _unitOfWork.CompleteAsync();
                var mappedSubcategory = _mapper.Map<SubcategoryResultDto>(subcategory);
                return response.Success(mappedSubcategory);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }
    }
}
