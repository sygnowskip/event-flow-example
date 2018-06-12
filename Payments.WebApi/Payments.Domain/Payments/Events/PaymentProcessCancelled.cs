using System;
using EventFlow.Aggregates;

namespace Payments.Domain.Payments.Events
{
    public class PaymentProcessCancelled : AggregateEvent<PaymentAggregate, PaymentId>
    {
        public PaymentProcessCancelled(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
        public PaymentStatus Status { get; } = PaymentStatus.Cancelled;
    }
}