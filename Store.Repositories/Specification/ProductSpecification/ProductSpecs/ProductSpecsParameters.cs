using Store.Data.Entities.ProductEntities;

namespace Store.Repositories.Specification.ProductSpecification.ProductSpecs
{
    public class ProductSpecsParameters
    {
        public int? CategoryId { get; set; }

        public int? SubcategoryId { get; set; }
        //public List<ProductColorJoin> ProductColorJoins { get; set; } = new();
        //public List<ProductSizeJoin> ProductSizeJoins { get; set; } = new();
        public int? DiscountId { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        private const int MAX_PAGE_SIZE = 50;
        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
        }
        private string? _search;
        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower();
        }
    }
}
