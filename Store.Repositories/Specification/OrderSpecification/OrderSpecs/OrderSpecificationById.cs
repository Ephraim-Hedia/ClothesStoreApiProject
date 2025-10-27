using Store.Data.Entities.OrderEntities;
using System.Linq.Expressions;

namespace Store.Repositories.Specification.OrderSpecification.OrderSpecs
{
    public class OrderSpecificationById : BaseSpecification<Order>
    {
        public OrderSpecificationById(int orderId) 
            : base(order => order.Id == orderId)
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude("DeliveryMethod.ShippingAddress");
            AddInclude("DeliveryMethod.ShippingAddress.City");
            AddInclude(order => order.OrderItems);
        }
    }
}
