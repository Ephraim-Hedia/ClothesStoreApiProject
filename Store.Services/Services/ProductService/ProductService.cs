using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.ProductEntities;
using Store.Repositories.Interfaces;
using Store.Repositories.Specification.ProductSpecification.ProductSpecs;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Helper.Validation;
using Store.Services.Services.ProductService.Dtos;

namespace Store.Services.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        private readonly EntityListValidator _listValidator;
        private readonly EntityValidator _validator;
        public ProductService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ProductService> logger,
            EntityListValidator listValidator,
            EntityValidator validator
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _listValidator = listValidator;
            _validator = validator;
        }
        public async Task<CommonResponse<ProductResultDto>> AddProductAsync(ProductCreateDto dto)
        {
            var response = new CommonResponse<ProductResultDto>();

            if (dto == null)
                return response.Fail("400", "Invalid product data");

            // 1) Validate Category
            var categoryResult = await _validator.ValidateEntityAsync<Category>(dto.CategoryId);
            if (!categoryResult.IsValid)
                return response.Fail(categoryResult.ErrorCode!, categoryResult.ErrorMessage!);

            // 2) Validate Subcategory
            var subcategoryResult = await _validator.ValidateEntityAsync<Subcategory>(dto.SubcategoryId);
            if (!subcategoryResult.IsValid)
                return response.Fail(subcategoryResult.ErrorCode!, subcategoryResult.ErrorMessage!);

            // 3) Validate discount
            if(dto.DiscountId  != null)
            {
                var discountResult = await _validator.ValidateEntityAsync<Discount>(dto.DiscountId);
                if (!discountResult.IsValid)
                    return response.Fail(discountResult.ErrorCode!, discountResult.ErrorMessage!);
            }
            // 4) Validate Price 
            if (dto.Price <= 0)
                return response.Fail("400", "Price must be greater than zero.");

            // Validate Colors
            var colorValidation = await _listValidator.ValidateEntityIdsAsync<ProductColor>(dto.ProductColorIds);
            if (!colorValidation.IsValid)
                return response.Fail("400", colorValidation.ErrorMessage);

            // Validate Sizes
            var sizeValidation = await _listValidator.ValidateEntityIdsAsync<ProductSize>(dto.ProductSizeIds);
            if (!sizeValidation.IsValid)
                return response.Fail("400", sizeValidation.ErrorMessage);

            try
            {
                // After mapping dto → product
                var product = _mapper.Map<Product>(dto);

                if (dto.ProductColorIds?.Any() == true)
                {
                    product.ProductColorJoins = dto.ProductColorIds
                        .Distinct()
                        .Select(id => new ProductColorJoin
                        {
                            ProductColorId = id,
                            Product = product
                        })
                        .ToList();
                }

                if (dto.ProductSizeIds?.Any() == true)
                {
                    product.ProductSizeJoins = dto.ProductSizeIds
                        .Distinct()
                        .Select(id => new ProductSizeJoin
                        {
                            ProductSizeId = id,
                            Product = product
                        })
                        .ToList();
                }

                await _unitOfWork.Repository<Product, int>().AddAsync(product);
                await _unitOfWork.CompleteAsync();

                var mapped = _mapper.Map<ProductResultDto>(product);
                return response.Success(mapped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding product {Name}", dto.Name);
                throw;
            }
        }

        public async Task<CommonResponse<bool>> DeleteProductAsync(int productId)
        {
            var response = new CommonResponse<bool>();

            if (productId <= 0)
                return response.Fail("400", "Invalid Data, Product Id must be greater than 0");

            try
            {
                var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(productId);

                if (product == null)
                    return response.Fail("404", "Product not found");

                _unitOfWork.Repository<Product, int>().Delete(product);
                await _unitOfWork.CompleteAsync();
                return response.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting product {ProductId}", productId);
                throw;
            }
        }

        public async Task<CommonResponse<IReadOnlyList<ProductResultDto>>> GetAllProductsAsync()
        {
            var response = new CommonResponse<IReadOnlyList<ProductResultDto>>();

            try
            {
                var specsParameters = new ProductSpecsParameters();
                var specs = new ProductSpecification(specsParameters);
                var products = await _unitOfWork.Repository<Product, int>()
                    .GetAllWithSpecificationAsync(specs);

                if (!products.Any())
                    return response.Fail("404", "No products found");

                var mapped = _mapper.Map<IReadOnlyList<ProductResultDto>>(products);
                return response.Success(mapped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all products");
                throw;
            }
        }

        public async Task<CommonResponse<ProductResultDto>> GetProductByIdAsync(int productId)
        {
            var response = new CommonResponse<ProductResultDto>();

            if (productId <= 0)
                return response.Fail("400", "Invalid Data, Product Id must be greater than 0");

            try
            {
                var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(productId);

                if (product == null)
                    return response.Fail("404", "Product not found");

                var mapped = _mapper.Map<ProductResultDto>(product);
                return response.Success(mapped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving product with ID {ProductId}", productId);
                throw;
            }
        }

        public async Task<CommonResponse<ProductResultDto>> UpdateProductAsync(int productId, ProductUpdateDto dto)
        {
            var response = new CommonResponse<ProductResultDto>();

            // Check about the product Id 
            var productResult = await _validator.ValidateEntityAsync<Product>(productId);
            if (!productResult.IsValid)
                return response.Fail(productResult.ErrorCode!, productResult.ErrorMessage!);

            if (dto == null)
                return response.Fail("404", "Product Data not found, cannot update Product");

            // 1) Validate Category
            if(dto.CategoryId != null)
            {
                var categoryResult = await _validator.ValidateEntityAsync<Category>(dto.CategoryId);
                if (!categoryResult.IsValid)
                    return response.Fail(categoryResult.ErrorCode!, categoryResult.ErrorMessage!);
            }
            // 2) Validate Subcategory
            if (dto.SubcategoryId != null)
            {
                var subcategoryResult = await _validator.ValidateEntityAsync<Subcategory>(dto.SubcategoryId);
                if (!subcategoryResult.IsValid)
                    return response.Fail(subcategoryResult.ErrorCode!, subcategoryResult.ErrorMessage!);
            }
            // 3) Validate discount
            if (dto.DiscountId != null)
            {
                var discountResult = await _validator.ValidateEntityAsync<Discount>(dto.DiscountId);
                if (!discountResult.IsValid)
                    return response.Fail(discountResult.ErrorCode!, discountResult.ErrorMessage!);
            }
            // 4) Validate Price 
            if (dto.Price != null)
            {
                if (dto.Price <= 0)
                    return response.Fail("400", "Price must be greater than zero.");
            }

            // Reuse same validator helper for colors & sizes
            var colorValidation = await _listValidator.ValidateEntityIdsAsync<ProductColor>(dto.ProductColorIds);
            if (!colorValidation.IsValid)
                return response.Fail("400", colorValidation.ErrorMessage);

            var sizeValidation = await _listValidator.ValidateEntityIdsAsync<ProductSize>(dto.ProductSizeIds);
            if (!sizeValidation.IsValid)
                return response.Fail("400", sizeValidation.ErrorMessage);

            try
            {
                var product = productResult.Entity;
                _mapper.Map(dto, product);

                // Update join tables
                if (dto.ProductColorIds?.Any() == true)
                {
                    product.ProductColorJoins = dto.ProductColorIds
                        .Select(id => new ProductColorJoin
                        {
                            ProductColorId = id,
                            Product = product
                        })
                        .ToList();
                }

                if (dto.ProductSizeIds?.Any() == true)
                {
                    product.ProductSizeJoins = dto.ProductSizeIds
                        .Select(id => new ProductSizeJoin
                        {
                            ProductSizeId = id,
                            Product = product
                        })
                        .ToList();
                }

                _unitOfWork.Repository<Product, int>().Update(product);
                await _unitOfWork.CompleteAsync();

                var mapped = _mapper.Map<ProductResultDto>(product);
                return response.Success(mapped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating product {ProductId}", productId);
                throw;
            }
        }
   
    }
}



