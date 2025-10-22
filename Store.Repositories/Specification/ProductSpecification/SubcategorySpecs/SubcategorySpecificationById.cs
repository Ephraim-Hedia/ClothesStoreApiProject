using Store.Data.Entities.ProductEntities;
using System.Linq.Expressions;

namespace Store.Repositories.Specification.ProductSpecification.SubcategorySpecs
{
    public class SubcategorySpecificationById : BaseSpecification<Subcategory>
    {
        public SubcategorySpecificationById(int id) 
            : base(subcategory => subcategory.Id == id)
        {
            AddInclude(subcategory => subcategory.Discount);
            AddInclude(subcategory => subcategory.Category);

        }
    }
}
