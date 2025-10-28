using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.BasketEntities;
using Store.Data.Entities.IdentityEntities;
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

            // Check about the city 
            var city = await _unitOfWork.Repository<City, int>().GetByIdAsync(dto.ShippingAddress.CityId);
            if (city == null)
                return response.Fail("404", "Not Found City");

            // Create Delivery & Shipping Address
            var shippingAddress = _mapper.Map<ShippingAddress>(dto.ShippingAddress);
            shippingAddress.City = city;
            await _unitOfWork.Repository<ShippingAddress, int>().AddAsync(shippingAddress);
            await _unitOfWork.CompleteAsync();

            var delivery = new Delivery
            {
                ShippingAddress = shippingAddress,
                DeliveryPrice = shippingAddress.City.DeliveryCost,
                EstimatedArrivalDate = DateTime.UtcNow.AddDays(shippingAddress.City.EstimatedDeliveryDays)
                // You can also add DeliveryMethodId if you have one
            };
            await _unitOfWork.Repository<Delivery, int>().AddAsync(delivery);
            await _unitOfWork.CompleteAsync();

            // Create the order
            var order = new Order
            {
                BuyerEmail = userId,
                DeliveryId = delivery.Id,
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

            var specs = new OrderSpecificationById(orderId);
            var order = await _unitOfWork.Repository<Order, int>().GetByIdWithSpecificationAsync(specs);
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

        public async Task<CommonResponse<bool>> CancelOrderAsync(int orderId, string userEmail)
        {
            
            var response = new CommonResponse<bool>();

            if (orderId <= 0)
                return response.Fail("400", "Invalid order ID");

            var specs = new OrderSpecificationById(orderId);
            var order = await _unitOfWork.Repository<Order, int>().GetByIdWithSpecificationAsync(specs);

            if (order == null || order.BuyerEmail != userEmail)
                return response.Fail("404", "Order not found or access denied");

            if (order.OrderStatus == OrderStatus.canceled)
                return response.Fail("400", "Order is already canceled");

            if (order.OrderStatus == OrderStatus.delivered)
                return response.Fail("400", "Delivered orders cannot be canceled");

            // Cancel the delivery as well
            
            var delivery = await _unitOfWork.Repository<Delivery, int>().GetByIdAsync(order.DeliveryId.Value);
            if (delivery == null)
                return response.Fail("404", "Associated delivery not found");
            try
            {
                delivery.UpdateStatus(DeliveryStatus.Canceled);
            }
            catch (InvalidOperationException ex)
            {
                return response.Fail("400", ex.Message);
            }

            order.OrderStatus = OrderStatus.canceled;

            _unitOfWork.Repository<Delivery, int>().Update(delivery);
            _unitOfWork.Repository<Order, int>().Update(order);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Order {OrderId} and its delivery were canceled by user {UserEmail}", orderId, userEmail);

            return response.Success(true);
        }

        // --------------------------
        // Update Delivery Info
        // --------------------------
        public async Task<CommonResponse<OrderResultDto>> UpdateDeliveryAsync(int orderId, UpdateDeliveryDto dto, string userEmail)
        {
            var response = new CommonResponse<OrderResultDto>();

            if (orderId <= 0)
                return response.Fail("400", "Invalid order ID");

            var specs = new OrderSpecificationById(orderId);
            var order = await _unitOfWork.Repository<Order, int>().GetByIdWithSpecificationAsync(specs);

            if (order == null || order.BuyerEmail != userEmail)
                return response.Fail("404", "Order not found or access denied");

            var delivery = await _unitOfWork.Repository<Delivery, int>().GetByIdAsync(order.DeliveryId.Value);
            if (delivery == null)
                return response.Fail("404", "Associated delivery not found");

            // Update delivery status if provided
            if (dto.Status.HasValue)
            {
                try
                {
                    delivery.UpdateStatus(dto.Status.Value);
                }
                catch (InvalidOperationException ex)
                {
                    return response.Fail("400", ex.Message);
                }
            }

            // Update courier info
            if (!string.IsNullOrEmpty(dto.CourierName) && !string.IsNullOrEmpty(dto.TrackingNumber))
                delivery.AssignCourier(dto.CourierName, dto.TrackingNumber);

            // Add note
            if (!string.IsNullOrEmpty(dto.Notes))
                delivery.AddNote(dto.Notes);

            _unitOfWork.Repository<Delivery, int>().Update(delivery);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Delivery for order {OrderId} updated successfully by {UserEmail}", orderId, userEmail);

            return response.Success(_mapper.Map<OrderResultDto>(order));
        }

        // --------------------------
        // Update Order Items (Before Processing)
        // --------------------------
        public async Task<CommonResponse<OrderResultDto>> UpdateOrderItemsQuantityAsync(int orderId, List<UpdateOrderItemDto> items, string userEmail)
        {
            var response = new CommonResponse<OrderResultDto>();

            if (orderId <= 0)
                return response.Fail("400", "Invalid order ID");

            var specs = new OrderSpecificationById(orderId);
            var order = await _unitOfWork.Repository<Order, int>().GetByIdWithSpecificationAsync(specs);

            if (order == null || order.BuyerEmail != userEmail)
                return response.Fail("404", "Order not found or access denied");

            var delivery = await _unitOfWork.Repository<Delivery, int>().GetByIdAsync(order.DeliveryId.Value);
            if (delivery == null)
                return response.Fail("404", "Associated delivery not found");

            // Big Note
            // Prevent editing items after processing starts
            if (delivery.Status != DeliveryStatus.Pending)
                return response.Fail("400", "Cannot modify order items after processing begins.");

            foreach (var dto in items)
            {
                var item = order.OrderItems.FirstOrDefault(i => i.Id == dto.ItemId);
                if (item == null)
                    continue;

                if (dto.Quantity <= 0)
                    _unitOfWork.Repository<OrderItem, int>().Delete(item);
                else
                    item.Quantity = dto.Quantity;
            }

            // Recalculate subtotal
            order.Subtotal = order.OrderItems.Sum(i => i.Price * i.Quantity);
            _unitOfWork.Repository<Order, int>().Update(order);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Items for order {OrderId} updated by {UserEmail}", orderId, userEmail);

            return response.Success(_mapper.Map<OrderResultDto>(order));
        }


        public async Task<CommonResponse<OrderResultDto>> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var response = new CommonResponse<OrderResultDto>();

            if (orderId <= 0)
                return response.Fail("400", "Invalid order ID");

            var specs = new OrderSpecificationById(orderId);
            var order = await _unitOfWork.Repository<Order, int>().GetByIdWithSpecificationAsync(specs);

            if (order == null)
                return response.Fail("404", "Order not found");

            // Prevent invalid transitions
            if (order.OrderStatus == OrderStatus.canceled)
                return response.Fail("400", "Cannot update a canceled order");

            if (order.OrderStatus == OrderStatus.delivered)
                return response.Fail("400", "Delivered orders cannot be updated");

            // If moving to delivered, ensure delivery is completed
            var delivery = await _unitOfWork.Repository<Delivery, int>().GetByIdAsync(order.DeliveryId.Value);
            if (delivery == null)
                return response.Fail("404", "Associated delivery not found");

            if (newStatus == OrderStatus.delivered && delivery.Status != DeliveryStatus.Delivered)
                return response.Fail("400", "Delivery must be completed before marking the order as delivered");

            order.OrderStatus = newStatus;
            _unitOfWork.Repository<Order, int>().Update(order);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Order {OrderId} status updated to {NewStatus}", orderId, newStatus);

            return response.Success(_mapper.Map<OrderResultDto>(order));
        }


        public async Task<CommonResponse<OrderResultDto>> AddItemToOrderAsync(int orderId, AddOrderItemDto dto, string userEmail)
        {
            var response = new CommonResponse<OrderResultDto>();

            if (orderId <= 0)
                return response.Fail("400", "Invalid order ID");

            if (dto == null || dto.ProductId <= 0 || dto.Quantity <= 0)
                return response.Fail("400", "Invalid item data");

            var specs = new OrderSpecificationById(orderId);
            var order = await _unitOfWork.Repository<Order, int>().GetByIdWithSpecificationAsync(specs);
            if (order == null || order.BuyerEmail != userEmail)
                return response.Fail("404", "Order not found or access denied");

            var delivery = await _unitOfWork.Repository<Delivery, int>().GetByIdAsync(order.DeliveryId.Value);
            if (delivery == null)
                return response.Fail("404", "Associated delivery not found");

            if (delivery.Status != DeliveryStatus.Pending)
                return response.Fail("400", "Cannot modify order after processing begins.");

            var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(dto.ProductId);
            if (product == null)
                return response.Fail("404", "Product not found");

            var existingItem = order.OrderItems.FirstOrDefault(i => i.ItemOrdered.ProductItemId == dto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                var orderItem = new OrderItem
                {
                    Price = product.Price,
                    Quantity = dto.Quantity,
                    ItemOrdered = new ProductOrdered
                    {
                        ProductItemId = product.Id,
                        ProductName = product.Name
                    }
                };

                order.OrderItems.Add(orderItem);
            }

            order.Subtotal = order.OrderItems.Sum(i => i.Price * i.Quantity);
            _unitOfWork.Repository<Order, int>().Update(order);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Item added to order {OrderId} by {UserEmail}", orderId, userEmail);

            return response.Success(_mapper.Map<OrderResultDto>(order));
        }


        public async Task<CommonResponse<OrderResultDto>> RemoveItemFromOrderAsync(int orderId, int itemId, string userEmail)
        {
            var response = new CommonResponse<OrderResultDto>();

            if (orderId <= 0 || itemId <= 0)
                return response.Fail("400", "Invalid input data");

            var specs = new OrderSpecificationById(orderId);
            var order = await _unitOfWork.Repository<Order, int>().GetByIdWithSpecificationAsync(specs);
            if (order == null || order.BuyerEmail != userEmail)
                return response.Fail("404", "Order not found or access denied");

            var delivery = await _unitOfWork.Repository<Delivery, int>().GetByIdAsync(order.DeliveryId.Value);
            if (delivery == null)
                return response.Fail("404", "Associated delivery not found");

            if (delivery.Status != DeliveryStatus.Pending)
                return response.Fail("400", "Cannot modify order after processing begins.");

            var item = order.OrderItems.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                return response.Fail("404", "Order item not found");

            _unitOfWork.Repository<OrderItem, int>().Delete(item);
            await _unitOfWork.CompleteAsync();

            // Recalculate subtotal
            order.Subtotal = order.OrderItems.Sum(i => i.Price * i.Quantity);
            _unitOfWork.Repository<Order, int>().Update(order);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Item {ItemId} removed from order {OrderId} by {UserEmail}", itemId, orderId, userEmail);

            return response.Success(_mapper.Map<OrderResultDto>(order));
        }

    }
}
