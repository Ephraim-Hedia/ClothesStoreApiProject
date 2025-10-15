using Store.Data.Entities.BasketEntities;
using System.Linq.Expressions;

namespace Store.Repositories.Specification.BasketSpecification.BasketSpecs
{
    public class BasketSpecification : BaseSpecification<Basket>
    {
        public BasketSpecification(string userId) 
            : base(basket => basket.UserId == userId)
        {
            AddInclude(b => b.BasketItems);
        }
    }
}
