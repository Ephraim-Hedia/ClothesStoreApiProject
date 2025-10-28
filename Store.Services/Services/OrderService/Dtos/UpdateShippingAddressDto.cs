namespace Store.Services.Services.OrderService.Dtos
{
    public class UpdateShippingAddressDto
    {
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public int CityId { get; set; }
    }
}
