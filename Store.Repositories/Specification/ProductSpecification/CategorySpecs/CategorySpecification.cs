using Store.Data.Entities.ProductEntities;

namespace Store.Repositories.Specification.ProductSpecification.CategorySpecs
{
    public class CategorySpecification : BaseSpecification<Category>
    {
        public CategorySpecification(string categoryName) 
            : base(category => category.Name.ToLower().Trim() == categoryName.ToLower().Trim())
        {
        }
        // General Specs 
        public CategorySpecification(CategorySpecsParameters specs)
            : base(category =>
            (string.IsNullOrEmpty(specs.Search) || category.Name.Trim().ToLower().Contains(specs.Search)) &&
            (!specs.DiscountId.HasValue || category.DiscountId == specs.DiscountId.Value)

            )
        {
            AddInclude(category => category.Subcategories);
            AddInclude(category => category.Discount);

            if (!string.IsNullOrEmpty(specs.Sort))
            {
                switch (specs.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(category => category.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(category => category.Name);
                        break;
                    default:
                        AddOrderBy(category => category.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(category => category.Id);
            }
        }
    }
}
