using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Queries;
using Payments.Domain.Payments;
using Payments.Domain.Payments.Commands;
using Payments.Domain.Payments.Queries;

namespace Payments.Application.Payments
{
    public interface IPaymentsApplicationService
    {
        Task<Uri> BeginPaymentProcessAsync(string country, string currency, string system, string externalId,
            string externalCallbackUrl, decimal amount);
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
            var paymentDetails = await _queryProcessor.ProcessAsync(new GetPaymentDetailsQuery(externalId), CancellationToken.None)
                .ConfigureAwait(false);
            if (paymentDetails != null)
            {
                throw new InvalidOperationException($"Payment for external id: {externalId} already exists!");
            }

            var beginPaymentProcessResult = await _commandBus.PublishAsync(new BeginPaymentProcessCommand(PaymentId.New, country, currency,
                system, externalId, externalCallbackUrl, amount), CancellationToken.None);

            if (!beginPaymentProcessResult.IsSuccess)
            {
                throw new InvalidOperationException("Begin payment process failed");
            }

            return beginPaymentProcessResult.RedirectUrl;
        }
    }
}