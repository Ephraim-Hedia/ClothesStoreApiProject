using Store.Data.Entities.OrderEntities;
using System.Linq.Expressions;

namespace Store.Repositories.Specification.OrderSpecification.OrderSpecs
{
    public class OrderSpecificationByBuyerEmail : BaseSpecification<Order>
    {
        public OrderSpecificationByBuyerEmail(string buyerEmail) 
            : base(order => order.BuyerEmail == buyerEmail)
        {
            AddInclude(order => order.Delivery);
            AddInclude(order => order.OrderItems);
        }
    }
}
