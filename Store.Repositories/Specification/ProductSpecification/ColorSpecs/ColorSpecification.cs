using Store.Data.Entities.ProductEntities;


namespace Store.Repositories.Specification.ProductSpecification.ColorSpecs
{
    public class ColorSpecification : BaseSpecification<ProductColor>
    {
        public ColorSpecification(string colorName) : 
            base(productcolor => productcolor.ColorName == colorName)
        {
        }

        // General Specs 
        public ColorSpecification(ColorSpecsParameters specs)
            : base(color =>
            (string.IsNullOrEmpty(specs.Search) || color.ColorName.Trim().ToLower().Contains(specs.Search))
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
                        AddOrderBy(color => color.ColorName);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(color => color.ColorName);
                        break;
                    default:
                        AddOrderBy(color => color.ColorName);
                        break;
                }
            }
            else
            {
                AddOrderBy(color => color.Id);
            }
        }
    }
}
