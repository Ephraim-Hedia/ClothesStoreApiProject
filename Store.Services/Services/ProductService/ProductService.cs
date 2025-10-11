using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.ProductService.Dtos;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.ProductEntities;
using Store.Repositories.Interfaces;

namespace Store.Services.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        public ProductService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ProductService> logger
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CommonResponse<ProductResultDto>> AddProductAsync(ProductCreateDto dto)
        {
            var response = new CommonResponse<ProductResultDto>();

            if (dto == null)
                return response.Fail("400", "Invalid product data");

            // 1) Validate Category
            var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(dto.CategoryId);
            if (category == null)
                return response.Fail("404", $"Category with ID '{dto.CategoryId}' not found.");

            
            // 2) Validate subcategory (if provided) and relationship to category
            if (dto.SubcategoryId > 0 )
            {
                var subcategory = await _unitOfWork.Repository<Subcategory, int>().GetByIdAsync(dto.SubcategoryId.Value);
                if (subcategory == null)
                    return response.Fail("404", $"Subcategory with ID '{dto.SubcategoryId}' not found.");
                if (subcategory.CategoryId == dto.CategoryId)
                    return response.Fail("400", $"Subcategory '{dto.SubcategoryId}' does not belong to Category '{dto.CategoryId}'.");
            }

            // 3) Validate Discount (if provided)
            if (dto.DiscountId > 0)
            {
                var discount = await _unitOfWork.Repository<Discount, int>().GetByIdAsync(dto.DiscountId.Value);
                if (discount == null)
                    return response.Fail("404", $"Discount with ID '{dto.DiscountId}' not found.");
            }

            // 4) Validate Price 
            if (dto.Price <= 0)
                return response.Fail("400", "Price must be greater than zero.");

            // 5) Validate ProductColorIds deeply
            var submittedColorIds = dto.ProductColorIds?
                .Where(id => id > 0)
                .Distinct()
                .ToList() ?? new List<int>();

            var colorsToLink = new List<ProductColor>();
            if (submittedColorIds.Any())
            {
                // Parallel fetch by id (uses multiple DB calls since generic repo lacks GetByIds)
                var colorTasks = submittedColorIds.Select(id => _unitOfWork.Repository<ProductColor,int>().GetByIdAsync(id));
                var fetchedColors = (await Task.WhenAll(colorTasks)).ToList();

                // Find missing ids
                var foundColorIds = fetchedColors.Where(c => c != null).Select(c => c.Id).ToList();
                var missingColorIds = submittedColorIds.Except(foundColorIds).ToList();
                if (missingColorIds.Any())
                    return response.Fail("400", $"Invalid color IDs: {string.Join(", ", missingColorIds)}");
                colorsToLink = fetchedColors.ToList();
            }

            // 6) Validate ProductSizeIds deeply (same pattern)
            var submittedSizeIds = dto.ProductSizeIds?
                .Where(id => id > 0)
                .Distinct()
                .ToList() ?? new List<int>();

            var sizesToLink = new List<ProductSize>();
            if (submittedSizeIds.Any())
            {
                var sizeTasks = submittedSizeIds.Select(id => _unitOfWork.Repository<ProductSize,int>().GetByIdAsync(id));
                var fetchedSizes = (await Task.WhenAll(sizeTasks)).ToList();

                var foundSizeIds = fetchedSizes.Where(s => s != null).Select(s => s.Id).ToList();
                var missingSizeIds = submittedSizeIds.Except(foundSizeIds).ToList();
                if (missingSizeIds.Any())
                    return response.Fail("400", $"Invalid size IDs: {string.Join(", ", missingSizeIds)}");

                sizesToLink = fetchedSizes.ToList();
            }

            // 7) Validate ImageUrls deeply
            //var submittedImageUrls = dto.ImageUrls?
            //    .Where(u => u > 0)
            //    .Distinct()
            //    .ToList() ?? new List<int>();

            // Basic constraints
            //const int maxImages = 10;
            //const int maxUrlLength = 300;
            //if (submittedImageUrls.Count > maxImages)
            //    return response.Fail("400", $"Maximum allowed images is {maxImages}.");

            //var invalidUrls = submittedImageUrls.Where(u => !Uri.IsWellFormedUriString(u, UriKind.Absolute)).ToList();
            //if (invalidUrls.Any())
            //    return response.Fail("400", $"Invalid image URLs: {string.Join(", ", invalidUrls)}");

            //var longUrls = submittedImageUrls.Where(u => u.Length > maxUrlLength).ToList();
            //if (longUrls.Any())
            //    return response.Fail("400", $"Image URL too long (max {maxUrlLength} chars).");

            // =================================================================================
            try
            {
                var product = _mapper.Map<Product>(dto);

                // To DO
                // Link existing colors
                // Link sizes
                // Create image entities (new records) and set product FK
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
                var products = await _unitOfWork.Repository<Product, int>()
                    .GetAllAsync();

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

            if (productId <= 0)
                return response.Fail("400", "Invalid Data, Product Id must be greater than 0");
            if (dto == null)
                return response.Fail("404", "Product Data not found, cannot update Product");

            // 1) Validate Category
            if (dto.CategoryId > 0)
            {
                var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(dto.CategoryId.Value);
                if (category == null)
                    return response.Fail("404", $"Category with ID '{dto.CategoryId}' not found.");
            }
            
            // 2) Validate subcategory (if provided) and relationship to category
            if (dto.SubcategoryId > 0)
            {
                var subcategory = await _unitOfWork.Repository<Subcategory, int>().GetByIdAsync(dto.SubcategoryId.Value);
                if (subcategory == null)
                    return response.Fail("404", $"Subcategory with ID '{dto.SubcategoryId}' not found.");
                if (subcategory.CategoryId == dto.CategoryId)
                    return response.Fail("400", $"Subcategory '{dto.SubcategoryId}' does not belong to Category '{dto.CategoryId}'.");
            }

            // 3) Validate Discount (if provided)
            if (dto.DiscountId > 0)
            {
                var discount = await _unitOfWork.Repository<Discount, int>().GetByIdAsync(dto.DiscountId.Value);
                if (discount == null)
                    return response.Fail("404", $"Discount with ID '{dto.DiscountId}' not found.");
            }
            // 4) Validate Price 
            if(dto.Price  != null)
            {
                if (dto.Price <= 0)
                    return response.Fail("400", "Price must be greater than zero.");
            }
            
            // 5) Validate ProductColorIds deeply
            var submittedColorIds = dto.ProductColorIds?
                .Where(id => id > 0)
                .Distinct()
                .ToList() ?? new List<int>();

            var colorsToLink = new List<ProductColor>();
            if (submittedColorIds.Any())
            {
                // Parallel fetch by id (uses multiple DB calls since generic repo lacks GetByIds)
                var colorTasks = submittedColorIds.Select(id => _unitOfWork.Repository<ProductColor, int>().GetByIdAsync(id));
                var fetchedColors = (await Task.WhenAll(colorTasks)).ToList();

                // Find missing ids
                var foundColorIds = fetchedColors.Where(c => c != null).Select(c => c.Id).ToList();
                var missingColorIds = submittedColorIds.Except(foundColorIds).ToList();
                if (missingColorIds.Any())
                    return response.Fail("400", $"Invalid color IDs: {string.Join(", ", missingColorIds)}");
                colorsToLink = fetchedColors.ToList();
            }

            // 6) Validate ProductSizeIds deeply (same pattern)
            var submittedSizeIds = dto.ProductSizeIds?
                .Where(id => id > 0)
                .Distinct()
                .ToList() ?? new List<int>();

            var sizesToLink = new List<ProductSize>();
            if (submittedSizeIds.Any())
            {
                var sizeTasks = submittedSizeIds.Select(id => _unitOfWork.Repository<ProductSize, int>().GetByIdAsync(id));
                var fetchedSizes = (await Task.WhenAll(sizeTasks)).ToList();

                var foundSizeIds = fetchedSizes.Where(s => s != null).Select(s => s.Id).ToList();
                var missingSizeIds = submittedSizeIds.Except(foundSizeIds).ToList();
                if (missingSizeIds.Any())
                    return response.Fail("400", $"Invalid size IDs: {string.Join(", ", missingSizeIds)}");

                sizesToLink = fetchedSizes.ToList();
            }

            // 7) Validate ImageUrls deeply
            //var submittedImageUrls = dto.ImageUrls?
            //    .Where(u => !string.IsNullOrWhiteSpace(u))
            //    .Select(u => u.Trim())
            //    .Distinct()
            //    .ToList() ?? new List<string>();

            //// Basic constraints
            //const int maxImages = 10;
            //const int maxUrlLength = 300;
            //if (submittedImageUrls.Count > maxImages)
            //    return response.Fail("400", $"Maximum allowed images is {maxImages}.");

            //var invalidUrls = submittedImageUrls.Where(u => !Uri.IsWellFormedUriString(u, UriKind.Absolute)).ToList();
            //if (invalidUrls.Any())
            //    return response.Fail("400", $"Invalid image URLs: {string.Join(", ", invalidUrls)}");

            //var longUrls = submittedImageUrls.Where(u => u.Length > maxUrlLength).ToList();
            //if (longUrls.Any())
            //    return response.Fail("400", $"Image URL too long (max {maxUrlLength} chars).");

            try
            {
                var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(productId);
                if (product == null)
                    return response.Fail("404", "Product not found");

                _mapper.Map(dto, product);

                // To DO
                // Link new colors if any change happen
                // Link new sizes if any change happen
                // Create new image entities (new records) and set product FK if any change happen

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
