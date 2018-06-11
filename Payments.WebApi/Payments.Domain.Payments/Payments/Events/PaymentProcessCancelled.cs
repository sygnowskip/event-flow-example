using EventFlow.Aggregates;

namespace Payments.Domain.Payments.Payments.Events
{
    public class PaymentProcessCancelled : AggregateEvent<PaymentAggregate, PaymentId>
    {
        public PaymentStatus Status { get; } = PaymentStatus.Cancelled;
    }
}