using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using Payments.Domain.Payments;
using Payments.Domain.Payments.Commands;

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

        public PaymentsApplicationService(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public async Task<Uri> BeginPaymentProcessAsync(string country, string currency, string system, string externalId,
            string externalCallbackUrl, decimal amount)
        {
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