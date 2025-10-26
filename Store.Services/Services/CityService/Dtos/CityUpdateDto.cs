namespace Store.Services.Services.CityService.Dtos
{
    public class CityUpdateDto
    {
        public string? Name { get; set; }
        // Delivery cost for orders in this city
        public decimal? DeliveryCost { get; set; }

        // Optional estimated delivery time in days
        public int? EstimatedDeliveryDays { get; set; }
    }
}
