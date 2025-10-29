namespace Store.Services.Services.AddressService.Dtos
{
    public class AddressResultDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string? BuildingNumber { get; set; }
        public string? FloorNumber { get; set; }
        public string? ApartmentNumber { get; set; }
        public string? Landmark { get; set; } // Optional: to help couriers (e.g. “near the hospital”)

        // More precise breakdown of location
        public string? District { get; set; } // Optional local area within city
        public CityResultDto City { get; set; }

        // optional for historical tracing
        public string? RecipientName { get; set; }
        public string? PhoneNumber { get; set; }
    }
    public class CityResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
