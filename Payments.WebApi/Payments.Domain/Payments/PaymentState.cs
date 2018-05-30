using EventFlow.Aggregates;
using Payments.Domain.Payments.Events;

namespace Payments.Domain.Payments
{
    public enum PaymentStatus
    {
        Started,
        Cancelled,
        Failed,
        Completed
    }

    public class PaymentState : AggregateState<PaymentAggregate, PaymentId, PaymentState>,
        IApply<PaymentProcessStarted>
    {
        public PaymentStatus Status { get; private set; }
        public string Country { get; private set; }
        public string Currency { get; private set; }
        public string System { get; private set; }
        public decimal Amount { get; private set; }
        public string ExternalId { get; private set; }
        public string ExternalCallbackUrl { get; private set; }

        public void Apply(PaymentProcessStarted aggregateEvent)
        {
            Country = aggregateEvent.Country;
            Currency = aggregateEvent.Currency;
            System = aggregateEvent.System;
            Amount = aggregateEvent.Amount;
            ExternalId = aggregateEvent.ExternalId;
            ExternalCallbackUrl = aggregateEvent.ExternalCallbackUrl;
            Status = PaymentStatus.Started;
        }
    }
}