using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.BasketEntities;
using Store.Data.Entities.OrderEntities;
using Store.Data.Entities.ProductEntities;
using Store.Repositories.Interfaces;
using Store.Repositories.Specification.BasketSpecification.BasketSpecs;
using Store.Repositories.Specification.OrderSpecification.OrderSpecs;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.OrderService.Dtos;

namespace Store.Services.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        public OrderService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CommonResponse<OrderResultDto>> CreateOrderAsync(string userId, OrderCreateDto dto)
        {
            var response = new CommonResponse<OrderResultDto>();

            if (string.IsNullOrEmpty(userId))
                return response.Fail("400", "Invalid user ID");

            if (dto == null || dto.BasketId <= 0)
                return response.Fail("400", "Invalid order data");

            // Get the user's basket
            var basketSpec = new BasketSpecificationById(dto.BasketId);
            var basket = await _unitOfWork.Repository<Basket, int>().GetByIdWithSpecificationAsync(basketSpec);

            if (basket == null || basket.BasketItems == null || !basket.BasketItems.Any())
                return response.Fail("404", "Basket not found or empty");

            // Convert basket items → order items
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.ProductId);
                if (product == null)
                    return response.Fail("404", $"Product with ID {item.ProductId} not found");

                var orderItem = new OrderItem
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ItemOrdered = new ProductOrdered
                    {
                        ProductItemId = item.ProductId,
                        ProductName = product.Name,
                        ProductColor = item.Color?.ColorName,
                        ProductSize = item.Size?.Name
                    }
                };

                orderItems.Add(orderItem);
            }

            // Calculate subtotal
            var subtotal = orderItems.Sum(i => i.Price * i.Quantity);


            // Create Delivery & Shipping Address
            var shippingAddress = _mapper.Map<ShippingAddress>(dto.ShippingAddress);

            var delivery = new Delivery
            {
                ShippingAddress = shippingAddress,
                // You can also add DeliveryMethodId if you have one
            };
            await _unitOfWork.Repository<Delivery, int>().AddAsync(delivery);
            await _unitOfWork.CompleteAsync();

            // Create the order
            var order = new Order
            {
                BuyerEmail = userId,
                DeliveryMethod = delivery,
                OrderItems = orderItems,
                Subtotal = subtotal,
                BasketId = dto.BasketId,
                OrderStatus = OrderStatus.pending
            };

            

            await _unitOfWork.Repository<Order, int>().AddAsync(order);
            await _unitOfWork.CompleteAsync();

            // Optionally clear basket after order
            _unitOfWork.Repository<BasketItem, int>().DeleteRange(basket.BasketItems);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Order created successfully for user {UserId}", userId);

            return response.Success(_mapper.Map<OrderResultDto>(order));
        }

        public async Task<CommonResponse<OrderResultDto>> GetOrderByIdAsync(int orderId, string userEmail)
        {
            var response = new CommonResponse<OrderResultDto>();

            if (orderId <= 0)
                return response.Fail("400", "Invalid order ID");

            var order = await _unitOfWork.Repository<Order, int>().GetByIdAsync(orderId);
            if (order == null || order.BuyerEmail != userEmail)
                return response.Fail("404", "Order not found or access denied");

            return response.Success(_mapper.Map<OrderResultDto>(order));
        }

        public async Task<CommonResponse<List<OrderResultDto>>> GetUserOrdersAsync(string userEmail)
        {
            var response = new CommonResponse<List<OrderResultDto>>();

            if (string.IsNullOrEmpty(userEmail))
                return response.Fail("400", "User email is required");

            var ordersSpecs = new OrderSpecificationByBuyerEmail(userEmail);
            var orders = await _unitOfWork.Repository<Order, int>().GetAllWithSpecificationAsync(ordersSpecs);
            if (!orders.Any())
                return response.Fail("404", "No orders found for this user");

            var mapped = _mapper.Map<List<OrderResultDto>>(orders);
            return response.Success(mapped);
        }
    }
}
