using EventFlow.Aggregates;
using Payments.Domain.Payments.Payments;

namespace Payments.Domain.Payments.Events
{
    public class PaymentProcessCancelled : AggregateEvent<PaymentAggregate, PaymentId>
    {
        public PaymentStatus Status { get; } = PaymentStatus.Cancelled;
    }
}