using Store.Data.Entities.ProductEntities;
using System.Linq.Expressions;

namespace Store.Repositories.Specification.ProductSpecification.DiscountSpecs
{
    public class DiscountWithRelationsSpecification : BaseSpecification<Discount>
    {
        public DiscountWithRelationsSpecification() : 
            base(null)
        {
            AddInclude(x => x.Categories);
            AddInclude(x => x.Subcategories);
            AddInclude(x => x.Products);
        }
    }
}
