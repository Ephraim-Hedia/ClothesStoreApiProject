using Store.Data.Entities.BasketEntities;
using System.Linq.Expressions;

namespace Store.Repositories.Specification.BasketSpecification.BasketSpecs
{
    public class BasketSpecificationById : BaseSpecification<Basket>
    {
        public BasketSpecificationById(int basketId) 
            : base(basket => basket.Id == basketId)
        {
            AddInclude(b => b.BasketItems);
            AddInclude($"{nameof(Basket.BasketItems)}.{nameof(BasketItem.Product)}");
            AddInclude($"{nameof(Basket.BasketItems)}.{nameof(BasketItem.Color)}");
            AddInclude($"{nameof(Basket.BasketItems)}.{nameof(BasketItem.Size)}");
        }
    }
}
