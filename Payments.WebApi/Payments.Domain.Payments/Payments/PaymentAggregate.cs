using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Snapshots;
using EventFlow.Snapshots.Strategies;
using Payments.Domain.Payments.Payments.Events;
using Payments.Domain.Payments.Payments.Snapshots;
using Payments.Domain.Payments.Providers;

namespace Payments.Domain.Payments.Payments
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

        public async Task<Uri> BeginPaymentProcessAsync(string country, string currency, string system, string externalId, string externalCallbackUrl,
            decimal amount)
        {
            var paymentProvider = _paymentProviderFactory.GetPaymentProvider(country, system, currency);
            var redirectUrl = await paymentProvider.BeginPaymentProcessAsync(new BeginPaymentProcessModel(
                Id.ToString(),
                country,
                currency,
                system,
                amount));

            Emit(new PaymentProcessStarted(country, currency, system, amount, externalId, externalCallbackUrl, redirectUrl));

            return PaymentState.RedirectUrl;
        }

        public void CancelPaymentProcess()
        {
            Emit(new PaymentProcessCancelled());

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