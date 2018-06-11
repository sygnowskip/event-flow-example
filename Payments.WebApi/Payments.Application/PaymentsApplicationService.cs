using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Queries;
using Payments.Domain.Payments.Commands;
using Payments.Domain.Payments.Queries;
using Payments.Domain.Payments;

namespace Payments.Application
{
    public interface IPaymentsApplicationService
    {
        Task<Uri> BeginPaymentProcessAsync(string country, string currency, string system, string externalId,
            string externalCallbackUrl, decimal amount);

        Task CancelPaymentProcessAsync(string externalId);
        Task PingAsync(string externalId);
    }
    public class PaymentsApplicationService : IPaymentsApplicationService
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;

        public PaymentsApplicationService(ICommandBus commandBus, IQueryProcessor queryProcessor)
        {
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
        }

        public async Task<Uri> BeginPaymentProcessAsync(string country, string currency, string system, string externalId,
            string externalCallbackUrl, decimal amount)
        {
            var paymentDetails = await _queryProcessor
                .ProcessAsync(new GetPaymentDetailsQuery(externalId), CancellationToken.None);
            if (paymentDetails != null)
            {
                throw new InvalidOperationException($"PaymentState for external id: {externalId} already exists!");
            }

            var beginPaymentProcessResult = await _commandBus.PublishAsync(new BeginPaymentProcessCommand(PaymentId.New, country, currency,
                system, externalId, externalCallbackUrl, amount), CancellationToken.None);

            if (!beginPaymentProcessResult.IsSuccess)
            {
                throw new InvalidOperationException("Begin paymentState process failed");
            }

            return beginPaymentProcessResult.RedirectUrl;
        }

        public async Task CancelPaymentProcessAsync(string externalId)
        {
            var paymentDetails = await _queryProcessor.ProcessAsync(new GetPaymentDetailsQuery(externalId), CancellationToken.None)
                .ConfigureAwait(false);
            if (paymentDetails == null)
            {
                throw new InvalidOperationException($"PaymentState for external id: {externalId} does not exists!");
            }

            var cancelPaymentProcessResult = await _commandBus.PublishAsync(new CancelPaymentProcessCommand(PaymentId.With(paymentDetails.PaymentId)), CancellationToken.None);

            if (!cancelPaymentProcessResult.IsSuccess)
            {
                throw new InvalidOperationException("PaymentState cancellation process failed");
            }
        }

        public async Task PingAsync(string externalId)
        {
            var paymentDetails = await _queryProcessor.ProcessAsync(new GetPaymentDetailsQuery(externalId), CancellationToken.None)
                .ConfigureAwait(false);
            if (paymentDetails == null)
            {
                throw new InvalidOperationException($"PaymentState for external id: {externalId} does not exists!");
            }

            var cancelPaymentProcessResult = await _commandBus.PublishAsync(new PingPaymentProcessCommand(PaymentId.With(paymentDetails.PaymentId)), CancellationToken.None);
            if (!cancelPaymentProcessResult.IsSuccess)
            {
                throw new InvalidOperationException("PaymentState ping process failed");
            }
        }
    }
}