namespace Store.Services.Services.ProductService.Dtos
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string? SubcategoryName { get; set; }
        public string ThumbnailUrl { get; set; } // e.g., first image

    }
}
