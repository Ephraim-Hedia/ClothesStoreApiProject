﻿
namespace Store.Services.Services.OrderService.Dtos
{
    public class AddOrderItemDto
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }

        public int Quantity { get; set; }
    }
}
