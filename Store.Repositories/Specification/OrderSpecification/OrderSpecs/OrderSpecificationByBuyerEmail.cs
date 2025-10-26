using Store.Data.Entities.BasketEntities;
using Store.Data.Entities.OrderEntities;
using System.Linq.Expressions;

namespace Store.Repositories.Specification.OrderSpecification.OrderSpecs
{
    public class OrderSpecificationByBuyerEmail : BaseSpecification<Order>
    {
        public OrderSpecificationByBuyerEmail(string buyerEmail) 
            : base(order => order.BuyerEmail == buyerEmail)
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude($"{nameof(Order.DeliveryMethod)}.{nameof(Delivery.ShippingAddress)}");
            AddInclude(order => order.OrderItems);
        }
    }
}
