using Store.Data.Entities.BasketEntities;
using System.Linq.Expressions;

namespace Store.Repositories.Specification.BasketSpecification.BasketSpecs
{
    public class BasketSpecificationByFingerPrint : BaseSpecification<Basket>
    {
        public BasketSpecificationByFingerPrint(string fingerPrint) 
            : base(basket =>basket.FingerPrint == fingerPrint)
        {
            AddInclude(b => b.BasketItems);
            AddInclude($"{nameof(Basket.BasketItems)}.{nameof(BasketItem.Product)}");
            AddInclude($"{nameof(Basket.BasketItems)}.{nameof(BasketItem.Color)}");
            AddInclude($"{nameof(Basket.BasketItems)}.{nameof(BasketItem.Size)}");
        }
    }
}
