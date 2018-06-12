using System;
using EventFlow.Aggregates;

namespace Payments.Domain.Payments.Events
{
    public class PaymentProcessCompleted : AggregateEvent<PaymentAggregate, PaymentId>
    {
        public PaymentProcessCompleted(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
        public PaymentStatus Status { get; } = PaymentStatus.Completed;
    }
}