using Store.Data.Entities;

namespace Store.Repositories.Specification.ProductSpecification
{
    public class CategorySpecification : BaseSpecification<Category>
    {
        public CategorySpecification(string categoryName) 
            : base(category => category.Name.ToLower().Trim() == categoryName.ToLower().Trim())
        {
        }
    }
}
