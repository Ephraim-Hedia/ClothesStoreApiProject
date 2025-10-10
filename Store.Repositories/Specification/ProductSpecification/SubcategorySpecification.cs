using Store.Data.Entities;

namespace Store.Repositories.Specification.ProductSpecification
{
    public class SubcategorySpecification : BaseSpecification<Subcategory>
    {
        public SubcategorySpecification(string subcategoryName) 
            : base(subcategory => subcategory.Name.ToLower().Trim() == subcategoryName.ToLower().Trim())
        {
        }
    }
}
