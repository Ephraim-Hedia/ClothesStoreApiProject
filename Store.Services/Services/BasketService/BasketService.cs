using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.BasketEntities;
using Store.Repositories.Interfaces;
using Store.Repositories.Specification.BasketSpecification.BasketItemSpecs;
using Store.Repositories.Specification.BasketSpecification.BasketSpecs;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.BasketService.Dtos;

namespace Store.Services.Services.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketService> _logger ;
        public BasketService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<BasketService> logger
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CommonResponse<BasketResultDto>> GetUserBasketAsync(string userId)
        {
            var response = new CommonResponse<BasketResultDto>();

            var basket = await LoadUserBasketAsync(userId);
            if (basket == null)
            {
                basket = new Basket { UserId = userId };
                await _unitOfWork.Repository<Basket, int>().AddAsync(basket);
                await _unitOfWork.CompleteAsync();
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
            
            var basket = await LoadUserBasketAsync(userId);
            if (basket == null)
            {
                basket = new Basket { UserId = userId };
                await _unitOfWork.Repository<Basket, int>().AddAsync(basket);
            }

            var existingItem = basket.BasketItems.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                var basketItem = _mapper.Map<BasketItem>(dto);
                basket.BasketItems.Add(basketItem);
            }

            await _unitOfWork.CompleteAsync();
            return response.Success(_mapper.Map<BasketResultDto>(basket));
        }

        public async Task<CommonResponse<bool>> ClearBasketAsync(string userId)
        {
            
            var response = new CommonResponse<bool>();
            if (string.IsNullOrEmpty(userId))
                return response.Fail("400", "Invalid Data, User Id is null");

            var basket = await LoadUserBasketAsync(userId);

            if (basket == null)
                return response.Fail("404", "Not Found basket");

            if (!basket.BasketItems.Any())
                return response.Fail("400", "Basket is empty");

            _unitOfWork.Repository<BasketItem, int>().DeleteRange(basket.BasketItems);
            await _unitOfWork.CompleteAsync();
            
            return response.Success(true);
        }


        public async Task<CommonResponse<bool>> RemoveItemAsync(string userId, int productId)
        {
            var response = new CommonResponse<bool>();
            if (string.IsNullOrEmpty(userId))
                return response.Fail("400", "Invalid Data, User Id is Null");
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

            return response.Success(true);
        }

        public async Task<CommonResponse<BasketResultDto>> UpdateQuantityAsync(string userId, int productId, int quantity)
        {
            var response = new CommonResponse<BasketResultDto>();

            if (string.IsNullOrEmpty(userId))
                return response.Fail("400", "Invalid Data, User Id is Null");
            if (productId <= 0)
                return response.Fail("400", "Invalid Data, Product Id must be more than 0");
            if (quantity <= 0)
                return response.Fail("400", "Invalid Data, quantity must be more than 0");

            
            var basket = await LoadUserBasketAsync(userId);
            if (basket == null)
                return response.Fail("404", "Basket Not Found");

            var basketItem = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);
            if (basketItem == null)
                return response.Fail("404", "Basket Item Not Found");

            basketItem.Quantity = quantity;
            _unitOfWork.Repository<BasketItem, int>().Update(basketItem);
            await _unitOfWork.CompleteAsync();
            
            var mappedBasket = _mapper.Map<BasketResultDto>(basket);
            return response.Success(mappedBasket);
        }
        private async Task<Basket?> LoadUserBasketAsync(string userId)
        {
            var spec = new BasketSpecification(userId);
            return await _unitOfWork.Repository<Basket, int>().GetByIdWithSpecificationAsync(spec);
        }
    }
}
