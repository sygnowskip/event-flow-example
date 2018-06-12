using EventFlow.Core;

namespace Payments.Domain.Orders
{
    public class OrderId : Identity<OrderId>
    {
        public OrderId(string value) : base(value)
        {
        }
    }
}