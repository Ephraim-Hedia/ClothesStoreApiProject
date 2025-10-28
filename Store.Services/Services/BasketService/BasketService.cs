using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.BasketEntities;
using Store.Data.Entities.ProductEntities;
using Store.Repositories.Interfaces;
using Store.Repositories.Specification.BasketSpecification.BasketSpecs;
using Store.Repositories.Specification.ProductSpecification.ProductSpecs;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.BasketService.Dtos;
using Store.Services.Services.UserService;

namespace Store.Services.Services.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketService> _logger ;
        private readonly IUserService _userService;
        public BasketService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<BasketService> logger,
            IUserService userService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }

        public async Task<CommonResponse<BasketResultDto>> GetUserBasketAsync(string? userId,string? fingerPrint)
        {
            Basket basket = new Basket();
            var response = new CommonResponse<BasketResultDto>();
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(fingerPrint))
                return response.Fail("400","Invalid Data, User Id and finger Print are Null");

            if(!string.IsNullOrEmpty(userId))
            {
                // 🧩 Validate User existence
                var result = await _userService.GetUserByIdAsync(userId);
                if (!result.IsSuccess)
                    return response.Fail("404", $"User with ID {userId} not found");
                basket = await LoadUserBasketByUserIdAsync(userId);
            }
            else
                basket = await LoadUserBasketByFingerPrintIdAsync(fingerPrint);


            if (basket == null)
            {
                basket = new Basket { UserId = userId,FingerPrint=fingerPrint };
                await _unitOfWork.Repository<Basket, int>().AddAsync(basket);
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("New basket created for user {UserId}", userId);
            }
            return response.Success(_mapper.Map<BasketResultDto>(basket));
        }
        public async Task<CommonResponse<BasketResultDto>> AddItemAsync(string? userId, string? fingerPrint, BasketItemCreateDto dto)
        {
            var response = new CommonResponse<BasketResultDto>();
            var basket = new Basket();
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(fingerPrint))
                return response.Fail("400", "Invalid Data, User Id and finger Print are Null");

            if (dto == null)
                return response.Fail("400", "Invalid Data, Basket Item is Null");
            if(dto.Quantity <=0 )
                return response.Fail("400", "Invalid Data, Quantity must be more than 0");

            if (!string.IsNullOrEmpty(userId))
            {
                // 🧩 Validate User existence
                var result = await _userService.GetUserByIdAsync(userId);
                if (!result.IsSuccess)
                    return response.Fail("404", $"User with ID {userId} not found");
                // 🧩 Load or create basket
                basket = await LoadUserBasketByUserIdAsync(userId);
            }
            else
                basket = await LoadUserBasketByFingerPrintIdAsync(fingerPrint);

            if (basket == null)
            {
                basket = new Basket { UserId = userId };
                await _unitOfWork.Repository<Basket, int>().AddAsync(basket);
            }

            // 🧩 Validate product existence
            var specs = new ProductSpecificationById(dto.ProductId);
            var product = await _unitOfWork.Repository<Product, int>().GetByIdWithSpecificationAsync(specs);
            if (product == null)
                return response.Fail("404", $"Product with ID {dto.ProductId} not found");

            // 🧩 Validate size existence
            var size = await _unitOfWork.Repository<ProductSize, int>().GetByIdAsync(dto.SizeId);
            if (size == null)
                return response.Fail("404", $"Size with ID {dto.SizeId} not found");

            // 🧩 Validate color existence
            var color = await _unitOfWork.Repository<ProductColor, int>().GetByIdAsync(dto.ColorId);
            if (color == null)
                return response.Fail("404", $"Color with ID {dto.ColorId} not found");

            // 🧩 Check if the item already exists in basket
            var existingItem = basket.BasketItems
                                        .FirstOrDefault(i=> i.ProductId == dto.ProductId &&
                                                            i.ColorId == dto.ColorId &&
                                                            i.SizeId == dto.SizeId);
            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
                _logger.LogInformation("Increased quantity of product {ProductId} for user {UserId}", dto.ProductId, userId);
            }
            else
            {
                var basketItem = _mapper.Map<BasketItem>(dto);
                basketItem.Price = product.GetPriceAfterBestDiscount(); // ✅ Always set price from product entity
                basket.BasketItems.Add(basketItem);
                _logger.LogInformation("Added product {ProductId} to basket for user {UserId}", dto.ProductId, userId);
            }

            await _unitOfWork.CompleteAsync();
            return response.Success(_mapper.Map<BasketResultDto>(basket));
        }
        public async Task<CommonResponse<bool>> ClearBasketAsync(string? userId, string? fingerPrint)
        {
            Basket basket = new Basket();
            var response = new CommonResponse<bool>();
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(fingerPrint))
                return response.Fail("400", "Invalid Data, User Id and finger Print are Null");

            if (!string.IsNullOrEmpty(userId))
            {
                // 🧩 Validate User existence
                var result = await _userService.GetUserByIdAsync(userId);
                if (!result.IsSuccess)
                    return response.Fail("404", $"User with ID {userId} not found");
                // 🧩 Load or create basket
                basket = await LoadUserBasketByUserIdAsync(userId);
            }
            else
                basket = await LoadUserBasketByFingerPrintIdAsync(fingerPrint);


            if (basket == null)
                return response.Fail("404", "Not Found basket");

            if (!basket.BasketItems.Any())
                return response.Fail("400", "Basket is empty");

            _unitOfWork.Repository<BasketItem, int>().DeleteRange(basket.BasketItems);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("Cleared basket for user {UserId}", userId);

            return response.Success(true);
        }
        public async Task<CommonResponse<bool>> RemoveItemAsync(string? userId, string? fingerPrint, int itemId)
        {
            Basket basket = new Basket(); 

            var response = new CommonResponse<bool>();
            if (itemId <= 0)
                return response.Fail("400", "Invalid Data, Product Id must be more than 0");

            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(fingerPrint))
                return response.Fail("400", "Invalid Data, User Id and finger Print are Null");

            if (!string.IsNullOrEmpty(userId))
            {
                // 🧩 Validate User existence
                var result = await _userService.GetUserByIdAsync(userId);
                if (!result.IsSuccess)
                    return response.Fail("404", $"User with ID {userId} not found");
                // 🧩 Load or create basket
                basket = await LoadUserBasketByUserIdAsync(userId);
            }
            else
                basket = await LoadUserBasketByFingerPrintIdAsync(fingerPrint);


            if (basket == null)
                return response.Fail("404", "Not Found Basket");

            var basketItem = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);
            if (basketItem == null)
                return response.Fail("404", "Basket Item not Found");

            _unitOfWork.Repository<BasketItem,int>().Delete(basketItem);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("Removed Basket Item {ItemId} from basket for user {UserId}", itemId, userId);
            return response.Success(true);
        }
        public async Task<CommonResponse<BasketResultDto>> UpdateQuantityAsync(string? userId, string? fingerPrint, int itemId, int quantity)
        {
            var response = new CommonResponse<BasketResultDto>();
            Basket basket = new Basket();

            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(fingerPrint))
                return response.Fail("400", "Invalid Data, User Id and finger Print are Null");

            if (!string.IsNullOrEmpty(userId))
            {
                // 🧩 Validate User existence
                var result = await _userService.GetUserByIdAsync(userId);
                if (!result.IsSuccess)
                    return response.Fail("404", $"User with ID {userId} not found");
                // 🧩 Load or create basket
                basket = await LoadUserBasketByUserIdAsync(userId);
            }
            else
                basket = await LoadUserBasketByFingerPrintIdAsync(fingerPrint);

            if (itemId <= 0)
                return response.Fail("400", "Invalid Data, Item Id must be more than 0");
            if (quantity < 0)
                return response.Fail("400", "Invalid Data, quantity must be more than 0");

            if (basket == null)
                return response.Fail("404", "Basket Not Found");

            var basketItem = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);
            if (basketItem == null)
                return response.Fail("404", "Basket Item Not Found");

            if (quantity == 0)
            {
                _unitOfWork.Repository<BasketItem, int>().Delete(basketItem);
                _logger.LogInformation("Removed Item {itemId} from basket for user {UserId}", itemId, userId);
            }
            
            else
            {
                basketItem.Quantity = quantity;
                _unitOfWork.Repository<BasketItem, int>().Update(basketItem);
                _logger.LogInformation("Updated quantity of Item {ItemId} to {Quantity} for user {UserId}", itemId, quantity, userId);
            }
            await _unitOfWork.CompleteAsync();
            return response.Success(_mapper.Map<BasketResultDto>(basket));
        }
        private async Task<Basket?> LoadUserBasketByUserIdAsync(string userId)
        {
            var spec = new BasketSpecification(userId);
            return await _unitOfWork.Repository<Basket, int>().GetByIdWithSpecificationAsync(spec);
        }
        private async Task<Basket?> LoadUserBasketByFingerPrintIdAsync(string fingerPrint)
        {
            var spec = new BasketSpecificationByFingerPrint(fingerPrint);
            return await _unitOfWork.Repository<Basket, int>().GetByIdWithSpecificationAsync(spec);
        }
    }
}
