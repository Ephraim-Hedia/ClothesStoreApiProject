namespace Store.Services.Services.DiscountService.Dtos
{
    public class DiscountResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public List<Discount_CategoryResultDto>? Categories { get; set; }
        public List<Discount_SubcategoryResultDto>? Subcategories { get; set; }
        public List<Discount_ProductResultDto>? Products { get; set; }
        public class Discount_CategoryResultDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Discount_SubcategoryResultDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Discount_ProductResultDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
