using Store.Data.Entities.ProductEntities;

namespace Store.Repositories.Specification.ProductSpecification.ProductSpecs
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecsParameters specs) 
            : base( product => 
            (string.IsNullOrEmpty(specs.Search) || product.Name.Trim().ToLower().Contains(specs.Search)) &&
            (string.IsNullOrEmpty(specs.Search) || product.Description.Trim().ToLower().Contains(specs.Search)) &&

            (!specs.DiscountId.HasValue || product.DiscountId == specs.DiscountId.Value) &&
            (!specs.SubcategoryId.HasValue || product.SubcategoryId == specs.SubcategoryId.Value) &&
            (!specs.CategoryId.HasValue || product.CategoryId == specs.CategoryId.Value))
        {
            AddInclude(p => p.Category);
            AddInclude("Category.Discount");
            AddInclude(p => p.Subcategory);
            AddInclude("Subcategory.Discount");
            AddInclude(p => p.Discount);
            AddInclude("ProductColorJoins.ProductColor");
            AddInclude("ProductSizeJoins.ProductSize");


        }
    }
}
