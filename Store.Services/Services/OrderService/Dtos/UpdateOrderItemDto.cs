namespace Store.Services.Services.OrderService.Dtos
{
    public class UpdateOrderItemDto
    {
        public int ItemId { get; set; }              // ID of the order item to update
        public int Quantity { get; set; }            // New quantity (0 means remove)
    }
}
