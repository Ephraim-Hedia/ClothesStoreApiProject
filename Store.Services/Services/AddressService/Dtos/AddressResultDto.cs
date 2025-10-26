namespace Store.Services.Services.AddressService.Dtos
{
    public class AddressResultDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public CityResultDto City { get; set; }

    }
    public class CityResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
