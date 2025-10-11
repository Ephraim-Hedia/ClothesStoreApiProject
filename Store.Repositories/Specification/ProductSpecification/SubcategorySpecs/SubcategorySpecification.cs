using Store.Data.Entities.ProductEntities;

namespace Store.Repositories.Specification.ProductSpecification.SubcategorySpecs
{
    public class SubcategorySpecification : BaseSpecification<Subcategory>
    {
        public SubcategorySpecification(string subcategoryName) 
            : base(subcategory => subcategory.Name.ToLower().Trim() == subcategoryName.ToLower().Trim())
        {
        }


        public SubcategorySpecification(SubcategorySpecsParameters specs)
            : base(subcategory =>
            (string.IsNullOrEmpty(specs.Search) || subcategory.Name.Trim().ToLower().Contains(specs.Search)) &&
            (!specs.DiscountId.HasValue || subcategory.DiscountId == specs.DiscountId.Value) &&
            (!specs.CategoryId.HasValue || subcategory.CategoryId == specs.CategoryId.Value) 
            )
        {
            
            //AddInclude(category => category.Type);
            //AddInclude(category => category.Brand);

            //ApplyPagination((specs.PageIndex - 1) * specs.PageSize, specs.PageSize);

            if (!string.IsNullOrEmpty(specs.Sort))
            {
                switch (specs.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(subcategory => subcategory.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(subcategory => subcategory.Name);
                        break;
                    default:
                        AddOrderBy(subcategory => subcategory.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(subcategory => subcategory.Id);
            }
        }
    }
}
