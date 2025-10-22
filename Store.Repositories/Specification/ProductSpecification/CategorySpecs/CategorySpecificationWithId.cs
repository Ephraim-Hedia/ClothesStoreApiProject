using Store.Data.Entities.ProductEntities;
using System.Linq.Expressions;

namespace Store.Repositories.Specification.ProductSpecification.CategorySpecs
{
    public class CategorySpecificationWithId : BaseSpecification<Category>
    {
        public CategorySpecificationWithId(int categoryId) 
            : base(category => category.Id == categoryId)
        {
            AddInclude(category => category.Discount);
            AddInclude(category => category.Subcategories);

        }
    }
}
