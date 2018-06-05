using System;
using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using EventFlow.Snapshots;
using EventFlow.Snapshots.Strategies;
using Payments.Domain.Payments.Events;
using Payments.Domain.Payments.Snapshots;
using Payments.Domain.Providers;

namespace Payments.Domain.Payments
{
    public class PaymentAggregate : SnapshotAggregateRoot<PaymentAggregate, PaymentId, PaymentAggregateSnapshot>
    {
        public const int SnapshotEveryVersion = 10;

        private readonly IPaymentProviderFactory _paymentProviderFactory;
        private readonly PaymentState _paymentState = new PaymentState();
        private readonly PaymentStateMachine _paymentStateMachine;

        public PaymentAggregate(PaymentId id, IPaymentProviderFactory paymentProviderFactory) : base(id,
            SnapshotEveryFewVersionsStrategy.With(SnapshotEveryVersion))
        {
            _paymentProviderFactory = paymentProviderFactory;
            _paymentStateMachine = new PaymentStateMachine(this);
            Register(_paymentState);
        }

        public PaymentStatus PaymentStatus => _paymentState.Status;

        public async Task<Uri> BeginPaymentProcessAsync(string country, string currency, string system, string externalId, string externalCallbackUrl,
            decimal amount)
        {
            await _paymentStateMachine.RaiseEvent(_paymentState, _paymentStateMachine.PaymentInitiated, new PaymentInitiatedEvent(country, currency, system, amount, externalId, externalCallbackUrl));
            return _paymentState.RedirectUrl;
        }

        public async Task CancelPaymentProcessAsync()
        {
            await _paymentStateMachine.RaiseEvent(_paymentState, _paymentStateMachine.PaymentCancelled);
        }

        public async Task PingAsync()
        {
            await _paymentStateMachine.RaiseEvent(_paymentState, _paymentStateMachine.PaymentPinged);
        }

        internal async Task BeginPaymentProcessAsyncInternal(string country, string currency, string system, string externalId, string externalCallbackUrl,
            decimal amount)
        {
            var paymentProvider = _paymentProviderFactory.GetPaymentProvider(country, system, currency);
            var redirectUrl = await paymentProvider.BeginPaymentProcessAsync(new BeginPaymentProcessModel(
                Id.ToString(),
                country,
                currency,
                system,
                amount));

            Emit(new PaymentProcessStarted(country, currency, system, amount, externalId, externalCallbackUrl, redirectUrl, _paymentStateMachine.InProgress.Name));
        }

        internal void CancelPaymentProcessAsyncInternal()
        {
            Emit(new PaymentProcessCancelled(_paymentStateMachine.Cancelled.Name));
        }

        internal void PingInternal()
        {
            Emit(new PaymentProcessPinged());
        }


        protected override Task<PaymentAggregateSnapshot> CreateSnapshotAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new PaymentAggregateSnapshot(_paymentState));
        }

        protected override Task LoadSnapshotAsync(PaymentAggregateSnapshot snapshot, ISnapshotMetadata metadata,
            CancellationToken cancellationToken)
        {
            _paymentState.Load(snapshot.PaymentState);
            return Task.CompletedTask;
        }
    }
}