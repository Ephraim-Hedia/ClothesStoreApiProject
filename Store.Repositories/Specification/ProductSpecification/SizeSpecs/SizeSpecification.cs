using Store.Data.Entities.ProductEntities;

namespace Store.Repositories.Specification.ProductSpecification.SizeSpecs
{
    public class SizeSpecification : BaseSpecification<ProductSize>
    {
        public SizeSpecification(string sizeName) : 
            base(size => size.Name == sizeName)
        {
        }

        // General Specs 
        public SizeSpecification(SizeSpecsWithParameters specs)
            : base(size =>
            (string.IsNullOrEmpty(specs.Search) || size.Name.Trim().ToLower().Contains(specs.Search))
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
                        AddOrderBy(size => size.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(size => size.Name);
                        break;
                    default:
                        AddOrderBy(size => size.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(size => size.Id);
            }
        }
    }
}
