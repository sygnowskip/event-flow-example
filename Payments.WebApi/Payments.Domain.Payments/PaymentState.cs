using System;
using EventFlow.Aggregates;
using Payments.Domain.Payments.Events;

namespace Payments.Domain.Payments.Payments
{
    public enum PaymentStatus
    {
        Started = 3,
        Cancelled,
        Completed
    }

    public class PaymentState : AggregateState<PaymentAggregate, PaymentId, PaymentState>,
        IApply<PaymentProcessStarted>,
        IApply<PaymentProcessCancelled>,
        IApply<PaymentProcessPinged>
    {
        public PaymentStatus Status { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string System { get; set; }
        public decimal Amount { get; set; }
        public string ExternalId { get; set; }
        public string ExternalCallbackUrl { get; set; }
        public int Ping { get; set; }
        public Uri RedirectUrl { get; set; }

        internal void Load(PaymentState toLoad)
        {
            Status = toLoad.Status;
            Country = toLoad.Country;
            Currency = toLoad.Currency;
            System = toLoad.System;
            Amount = toLoad.Amount;
            ExternalId = toLoad.ExternalId;
            ExternalCallbackUrl = toLoad.ExternalCallbackUrl;
            Ping = toLoad.Ping;
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
            RedirectUrl = aggregateEvent.RedirectUrl;
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