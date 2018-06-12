using System;
using EventFlow.Aggregates;
using EventFlow.MsSql.ReadStores.Attributes;
using EventFlow.ReadStores;
using Payments.Domain.Payments.Events;

namespace Payments.Domain.Payments.ReadModels
{
    public class PaymentDetailsReadModel : IReadModel,
        IAmReadModelFor<PaymentAggregate, PaymentId, PaymentProcessStarted>,
        IAmReadModelFor<PaymentAggregate, PaymentId, PaymentProcessCancelled>,
        IAmReadModelFor<PaymentAggregate, PaymentId, PaymentProcessCompleted>
    {
        [MsSqlReadModelIdentityColumn]
        public string PaymentId { get; private set; }
        public Guid OrderId { get; private set; }
        public string Username { get; private set; }
        public decimal TotalPrice { get; private set; }
        public PaymentStatus Status { get; private set; }
        [MsSqlReadModelVersionColumn]
        public int Version { get; set; }


        public void Apply(IReadModelContext context, IDomainEvent<PaymentAggregate, PaymentId, PaymentProcessStarted> domainEvent)
        {
            PaymentId = domainEvent.AggregateIdentity.Value;
            Status = domainEvent.AggregateEvent.Status;
            TotalPrice = domainEvent.AggregateEvent.TotalPrice;
            Username = domainEvent.AggregateEvent.Username;
            OrderId = domainEvent.AggregateEvent.OrderId;
        }

        public void Apply(IReadModelContext context, IDomainEvent<PaymentAggregate, PaymentId, PaymentProcessCancelled> domainEvent)
        {
            Status = domainEvent.AggregateEvent.Status;
        }

        public void Apply(IReadModelContext context, IDomainEvent<PaymentAggregate, PaymentId, PaymentProcessCompleted> domainEvent)
        {
            Status = domainEvent.AggregateEvent.Status;
        }
    }
}