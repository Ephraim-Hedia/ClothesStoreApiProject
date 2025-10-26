using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.CityService.Dtos
{
    public class CityCreateDto
    {
        [Required]
        public string Name { get; set; }
        // Delivery cost for orders in this city
        [Required]
        public decimal DeliveryCost { get; set; }

        // Optional estimated delivery time in days
        [Required]
        public int EstimatedDeliveryDays { get; set; }

    }
}
