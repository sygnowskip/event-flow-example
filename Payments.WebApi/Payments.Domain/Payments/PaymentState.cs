using EventFlow.Aggregates;
using Payments.Domain.Payments.Events;

namespace Payments.Domain.Payments
{
    public enum PaymentStatus
    {
        Started = 1,
        Cancelled,
        Failed,
        Completed
    }

    public class PaymentState : AggregateState<PaymentAggregate, PaymentId, PaymentState>,
        IApply<PaymentProcessStarted>,
        IApply<PaymentProcessCancelled>,
        IApply<PaymentProcessPinged>
    {
        public PaymentStatus Status { get; private set; }
        public string Country { get; private set; }
        public string Currency { get; private set; }
        public string System { get; private set; }
        public decimal Amount { get; private set; }
        public string ExternalId { get; private set; }
        public string ExternalCallbackUrl { get; private set; }
        public int Ping { get; private set; }

        internal void Load(PaymentState stateToLoad)
        {
            Status = stateToLoad.Status;
            Country = stateToLoad.Country;
            Currency = stateToLoad.Currency;
            System = stateToLoad.System;
            Amount = stateToLoad.Amount;
            ExternalId = stateToLoad.ExternalId;
            ExternalCallbackUrl = stateToLoad.ExternalCallbackUrl;
            Ping = stateToLoad.Ping;
        }

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

        public void Apply(PaymentProcessCancelled aggregateEvent)
        {
            Status = PaymentStatus.Cancelled;
        }

        public void Apply(PaymentProcessPinged aggregateEvent)
        {
            Ping++;
        }
    }
}