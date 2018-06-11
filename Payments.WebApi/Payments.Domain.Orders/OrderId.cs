using System;
using EventFlow.Core;

namespace Payments.Domain.Orders
{
    public class OrderId : Identity<OrderId>
    {
        public OrderId(Guid orderId) : base(orderId.ToString())
        {
        }
    }
}