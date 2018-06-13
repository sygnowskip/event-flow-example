using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Snapshots;
using EventFlow.Snapshots.Strategies;
using Payments.Domain.Payments.Events;
using Payments.Domain.Payments.Providers;
using Payments.Domain.Payments.Snapshots;
using Payments.Domain.Common.Aggregate;

namespace Payments.Domain.Payments
{
    public class PaymentAggregate : SnapshotAggregateRoot<PaymentAggregate, PaymentId, PaymentAggregateSnapshot>
    {
        public const int SnapshotEveryVersion = 10;

        private readonly IPaymentProviderFactory _paymentProviderFactory;
        public PaymentState PaymentState { get; } = new PaymentState();

        public int StateMachineState
        {
            get => (int)PaymentState.Status;
            set => PaymentState.Status = (PaymentStatus)value;
        }

        public PaymentAggregate(PaymentId id, IPaymentProviderFactory paymentProviderFactory) : base(id,
            SnapshotEveryFewVersionsStrategy.With(SnapshotEveryVersion))
        {
            _paymentProviderFactory = paymentProviderFactory;
            Register(PaymentState);
        }

        public async Task<Uri> BeginPaymentProcessAsync(Guid orderId, string username, decimal totalPrice)
        {
            var paymentProvider = _paymentProviderFactory.GetPaymentProvider();
            var redirectUrl = await paymentProvider.BeginPaymentProcessAsync(new BeginPaymentProcessModel(orderId, username, totalPrice));

            Emit(new PaymentProcessStarted(orderId, username, totalPrice, redirectUrl));

            return PaymentState.RedirectUrl;
        }

        public void CancelPaymentProcess()
        {
            Emit(new PaymentProcessCancelled(PaymentState.OrderId), this.MetadataFor(new { PaymentState.OrderId }));
        }
        public void CompletePaymentProcess()
        {
            Emit(new PaymentProcessCompleted(PaymentState.OrderId), this.MetadataFor(new { PaymentState.OrderId }));
        }

        public void Ping()
        {
            Emit(new PaymentProcessPinged());
        }

        protected override Task<PaymentAggregateSnapshot> CreateSnapshotAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new PaymentAggregateSnapshot(PaymentState));
        }

        protected override Task LoadSnapshotAsync(PaymentAggregateSnapshot snapshot, ISnapshotMetadata metadata,
            CancellationToken cancellationToken)
        {
            PaymentState.Load(snapshot.PaymentState);
            return Task.CompletedTask;
        }
    }
}