using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.BasketEntities;
using Store.Data.Entities.IdentityEntities;
using Store.Data.Entities.ProductEntities;
using Store.Repositories.Interfaces;
using Store.Repositories.Specification.BasketSpecification.BasketItemSpecs;
using Store.Repositories.Specification.BasketSpecification.BasketSpecs;
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

        public async Task<CommonResponse<BasketResultDto>> GetUserBasketAsync(string userId)
        {
            var response = new CommonResponse<BasketResultDto>();
            if (string.IsNullOrEmpty(userId))
                return response.Fail("400","Invalid Data, User Id is Null");

            // 🧩 Validate User existence
            var result = await _userService.GetUserByIdAsync(userId);
            if (!result.IsSuccess)
                return response.Fail("404", $"User with ID {userId} not found");

            var basket = await LoadUserBasketAsync(userId);
            if (basket == null)
            {
                basket = new Basket { UserId = userId };
                await _unitOfWork.Repository<Basket, int>().AddAsync(basket);
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("New basket created for user {UserId}", userId);
            }
            return response.Success(_mapper.Map<BasketResultDto>(basket));
        }
        public async Task<CommonResponse<BasketResultDto>> AddItemAsync(string userId, BasketItemCreateDto dto)
        {
            var response = new CommonResponse<BasketResultDto>();
            if (string.IsNullOrEmpty(userId))
                return response.Fail("400", "Invalid Data, user Id is Null");

            if (dto == null)
                return response.Fail("400", "Invalid Data, Basket Item is Null");
            if(dto.Quantity <=0 )
                return response.Fail("400", "Invalid Data, Quantity must be more than 0");

            // 🧩 Validate User existence
            var result = await _userService.GetUserByIdAsync(userId);
            if (!result.IsSuccess)
                return response.Fail("404", $"User with ID {userId} not found");

            // 🧩 Load or create basket
            var basket = await LoadUserBasketAsync(userId);
            if (basket == null)
            {
                basket = new Basket { UserId = userId };
                await _unitOfWork.Repository<Basket, int>().AddAsync(basket);
            }

            // 🧩 Validate product existence
            var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(dto.ProductId);
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
                basketItem.Price = product.Price; // ✅ Always set price from product entity
                basket.BasketItems.Add(basketItem);
                _logger.LogInformation("Added product {ProductId} to basket for user {UserId}", dto.ProductId, userId);
            }

            await _unitOfWork.CompleteAsync();
            return response.Success(_mapper.Map<BasketResultDto>(basket));
        }
        public async Task<CommonResponse<bool>> ClearBasketAsync(string userId)
        {
            
            var response = new CommonResponse<bool>();
            if (string.IsNullOrEmpty(userId))
                return response.Fail("400", "Invalid Data, User Id is null");

            // 🧩 Validate User existence
            var result = await _userService.GetUserByIdAsync(userId);
            if (!result.IsSuccess)
                return response.Fail("404", $"User with ID {userId} not found");

            var basket = await LoadUserBasketAsync(userId);
            if (basket == null)
                return response.Fail("404", "Not Found basket");

            if (!basket.BasketItems.Any())
                return response.Fail("400", "Basket is empty");

            _unitOfWork.Repository<BasketItem, int>().DeleteRange(basket.BasketItems);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("Cleared basket for user {UserId}", userId);

            return response.Success(true);
        }
        public async Task<CommonResponse<bool>> RemoveItemAsync(string userId, int productId)
        {
            var response = new CommonResponse<bool>();
            if (string.IsNullOrEmpty(userId))
                return response.Fail("400", "Invalid Data, User Id is Null");

            // 🧩 Validate User existence
            var result = await _userService.GetUserByIdAsync(userId);
            if (!result.IsSuccess)
                return response.Fail("404", $"User with ID {userId} not found");

            if (productId <= 0)
                return response.Fail("400", "Invalid Data, Product Id must be more than 0");

            var basket = await LoadUserBasketAsync(userId);

            if (basket == null)
                return response.Fail("404", "Not Found Basket");

            var basketItem = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);
            if (basketItem == null)
                return response.Fail("404", "Basket Item not Found");

            _unitOfWork.Repository<BasketItem,int>().Delete(basketItem);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("Removed product {ProductId} from basket for user {UserId}", productId, userId);
            return response.Success(true);
        }
        public async Task<CommonResponse<BasketResultDto>> UpdateQuantityAsync(string userId, int productId, int quantity)
        {
            var response = new CommonResponse<BasketResultDto>();

            if (string.IsNullOrEmpty(userId))
                return response.Fail("400", "Invalid Data, User Id is Null");
            // 🧩 Validate User existence
            var result = await _userService.GetUserByIdAsync(userId);
            if (!result.IsSuccess)
                return response.Fail("404", $"User with ID {userId} not found");

            if (productId <= 0)
                return response.Fail("400", "Invalid Data, Product Id must be more than 0");
            if (quantity < 0)
                return response.Fail("400", "Invalid Data, quantity must be more than 0");

            
            var basket = await LoadUserBasketAsync(userId);
            if (basket == null)
                return response.Fail("404", "Basket Not Found");

            var basketItem = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);
            if (basketItem == null)
                return response.Fail("404", "Basket Item Not Found");

            if (quantity == 0)
            {
                _unitOfWork.Repository<BasketItem, int>().Delete(basketItem);
                _logger.LogInformation("Removed product {ProductId} from basket for user {UserId}", productId, userId);
            }
            
            else
            {
                basketItem.Quantity = quantity;
                _unitOfWork.Repository<BasketItem, int>().Update(basketItem);
                _logger.LogInformation("Updated quantity of product {ProductId} to {Quantity} for user {UserId}", productId, quantity, userId);
            }
            await _unitOfWork.CompleteAsync();
            return response.Success(_mapper.Map<BasketResultDto>(basket));
        }
        private async Task<Basket?> LoadUserBasketAsync(string userId)
        {
            var spec = new BasketSpecification(userId);
            return await _unitOfWork.Repository<Basket, int>().GetByIdWithSpecificationAsync(spec);
        }
    }
}
