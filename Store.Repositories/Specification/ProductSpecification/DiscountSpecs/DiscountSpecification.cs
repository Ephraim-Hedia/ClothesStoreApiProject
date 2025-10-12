using Store.Data.Entities.ProductEntities;

namespace Store.Repositories.Specification.ProductSpecification.DiscountSpecs
{
    public class DiscountSpecification : BaseSpecification<Discount>
    {
        public DiscountSpecification(string discountName) 
            : base(discount => discount.Name.ToLower().Trim() == discountName.ToLower().Trim())
        {
        } 

        // General Specs 
        public DiscountSpecification(DiscountSpecsParameters specs)
            : base(discount =>
            (string.IsNullOrEmpty(specs.Search) || discount.Name.Trim().ToLower().Contains(specs.Search)) &&
            ((!specs.Percentage.HasValue ) || (discount.Percentage == specs.Percentage.Value))
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
                        AddOrderBy(discount => discount.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(discount => discount.Name);
                        break;
                    default:
                        AddOrderBy(discount => discount.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(discount => discount.Id);
            }
        }
    }
}
