using System;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Extensions;
using EventFlow.Queries;
using Payments.Domain.Payments.Events;
using Payments.Domain.Payments.Specifications;
using Payments.Domain.Providers;

namespace Payments.Domain.Payments
{
    public class PaymentAggregate : AggregateRoot<PaymentAggregate, PaymentId>
    {
        private readonly IPaymentProviderFactory _paymentProviderFactory;
        private readonly PaymentState _paymentState = new PaymentState();

        public PaymentAggregate(PaymentId id, IPaymentProviderFactory paymentProviderFactory) : base(id)
        {
            _paymentProviderFactory = paymentProviderFactory;
            Register(_paymentState);
        }

        public PaymentStatus PaymentStatus => _paymentState.Status;

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

            Emit(new PaymentProcessStarted(country, currency, system, amount, externalId, externalCallbackUrl));

            return redirectUrl;
        }

        public void CancelPaymentProcess()
        {
            //move it to Automatonymous
            new PaymentCancellationSpecification().ThrowDomainErrorIfNotSatisfied(this);

            Emit(new PaymentProcessCancelled());
        }
    }
}