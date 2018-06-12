using System;
using EventFlow.Aggregates;
using Payments.Domain.Payments.Events;

namespace Payments.Domain.Payments
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
        IApply<PaymentProcessCompleted>,
        IApply<PaymentProcessPinged>
    {
        public PaymentStatus Status { get; set; }
        public Guid OrderId { get; set; }
        public string Username { get; set; }
        public decimal TotalPrice { get; set; }
        public int Ping { get; set; }
        public Uri RedirectUrl { get; set; }

        internal void Load(PaymentState toLoad)
        {
            Status = toLoad.Status;
            TotalPrice = toLoad.TotalPrice;
            Username = toLoad.Username;
            OrderId = toLoad.OrderId;
            RedirectUrl = toLoad.RedirectUrl;
            Ping = toLoad.Ping;
        }

        public void Apply(PaymentProcessStarted aggregateEvent)
        {
            TotalPrice = aggregateEvent.TotalPrice;
            Username = aggregateEvent.Username;
            OrderId = aggregateEvent.OrderId;
            Status = PaymentStatus.Started;
            RedirectUrl = aggregateEvent.RedirectUrl;
        }

        public void Apply(PaymentProcessCancelled aggregateEvent)
        {
            Status = PaymentStatus.Cancelled;
            OrderId = aggregateEvent.OrderId;
        }

        public void Apply(PaymentProcessCompleted aggregateEvent)
        {
            Status = PaymentStatus.Completed;
            OrderId = aggregateEvent.OrderId;
        }

        public void Apply(PaymentProcessPinged aggregateEvent)
        {
            Ping++;
        }
    }
}