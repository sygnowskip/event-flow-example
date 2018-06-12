using System;
using EventFlow.Aggregates;

namespace Payments.Domain.Payments.Events
{
    public class PaymentProcessStarted : AggregateEvent<PaymentAggregate, PaymentId>
    {
        public PaymentProcessStarted(Guid orderId, string username, decimal totalPrice, Uri redirectUrl)
        {
            OrderId = orderId;
            Username = username;
            TotalPrice = totalPrice;
            RedirectUrl = redirectUrl;
        }

        public Guid OrderId { get; }
        public string Username { get; }
        public decimal TotalPrice { get; }
        public Uri RedirectUrl { get; }
        public PaymentStatus Status { get; } = PaymentStatus.Started;
    }
}