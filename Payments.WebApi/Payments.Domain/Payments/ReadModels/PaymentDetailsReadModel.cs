using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Payments.Domain.Payments.Events;

namespace Payments.Domain.Payments.ReadModels
{
    public class PaymentDetailsReadModel : IReadModel,
        IAmReadModelFor<PaymentAggregate, PaymentId, PaymentProcessStarted>
    {
        public PaymentId Id { get; private set; }
        public string Country { get; private set; }
        public string Currency { get; private set; }
        public string System { get; private set; }
        public decimal Amount { get; private set; }
        public string ExternalId { get; private set; }
        public string ExternalCallbackUrl { get; private set; }
        public PaymentStatus Status { get; private set; }


        public void Apply(IReadModelContext context, IDomainEvent<PaymentAggregate, PaymentId, PaymentProcessStarted> domainEvent)
        {
            Id = domainEvent.AggregateIdentity;
            Country = domainEvent.AggregateEvent.Country;
            System = domainEvent.AggregateEvent.System;
            Currency = domainEvent.AggregateEvent.Currency;
            Amount = domainEvent.AggregateEvent.Amount;
            ExternalId = domainEvent.AggregateEvent.ExternalId;
            ExternalCallbackUrl = domainEvent.AggregateEvent.ExternalCallbackUrl;
            Status = domainEvent.AggregateEvent.Status;
        }
    }
}