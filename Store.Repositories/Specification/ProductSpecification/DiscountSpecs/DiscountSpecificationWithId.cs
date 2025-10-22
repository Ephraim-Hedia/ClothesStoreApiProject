using Store.Data.Entities.ProductEntities;
using System.Linq.Expressions;

namespace Store.Repositories.Specification.ProductSpecification.DiscountSpecs
{
    public class DiscountSpecificationWithId : BaseSpecification<Discount>
    {
        public DiscountSpecificationWithId(int discountId) 
            : base(x => x.Id == discountId)
        {
            AddInclude(x => x.Categories);
            AddInclude(x => x.Subcategories);
            AddInclude(x => x.Products);
        }
    }
}
