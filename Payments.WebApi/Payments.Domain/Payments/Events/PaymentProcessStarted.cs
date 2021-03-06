﻿using EventFlow.Aggregates;

namespace Payments.Domain.Payments.Events
{
    public class PaymentProcessStarted : AggregateEvent<PaymentAggregate, PaymentId>
    {
        public PaymentProcessStarted(string country, string currency, string system, decimal amount, string externalId, string externalCallbackUrl)
        {
            Country = country;
            Currency = currency;
            System = system;
            Amount = amount;
            ExternalId = externalId;
            ExternalCallbackUrl = externalCallbackUrl;
        }

        public string Country { get; }
        public string Currency { get; }
        public string System { get; }
        public decimal Amount { get; }
        public string ExternalId { get; }
        public string ExternalCallbackUrl { get; }
        public PaymentStatus Status { get; } = PaymentStatus.Started;
    }
}