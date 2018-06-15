using System;
using EventFlow.Aggregates;

namespace Payments.Domain.Orders.Events
{
    public class OrderPaymentStarted : AggregateEvent<OrderAggregate, OrderId>
    {
        public OrderPaymentStarted(Guid paymentId)
        {
            PaymentId = paymentId;
        }

        public Guid PaymentId { get; }
    }
}