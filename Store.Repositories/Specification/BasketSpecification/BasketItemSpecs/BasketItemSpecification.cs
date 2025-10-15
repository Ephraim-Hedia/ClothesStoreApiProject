using Store.Data.Entities.BasketEntities;
using System.Linq.Expressions;

namespace Store.Repositories.Specification.BasketSpecification.BasketItemSpecs
{
    public class BasketItemSpecification : BaseSpecification<BasketItem>
    {
        public BasketItemSpecification(int productId) 
            : base(basketItem => basketItem.ProductId == productId)
        {
        }
    }
}
