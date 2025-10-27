using Store.Data.Entities.ProductEntities;
using System.Linq.Expressions;

namespace Store.Repositories.Specification.ProductSpecification.ProductSpecs
{
    public class ProductSpecificationById : BaseSpecification<Product>
    {
        public ProductSpecificationById(int productId) 
            : base(product => product.Id == productId)
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
