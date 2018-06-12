using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Queries;
using Payments.Domain.Payments.Commands;
using Payments.Domain.Payments.Queries;
using Payments.Domain.Payments;
using Payments.Domain.Payments.ReadModels;

namespace Payments.Application
{
    public interface IPaymentsApplicationService
    {
        Task CancelPaymentProcessAsync(Guid orderId);
        Task CompletePaymentProcessAsync(Guid orderId);
        Task PingAsync(Guid orderId);
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

        public async Task CancelPaymentProcessAsync(Guid orderId)
        {
            var paymentDetails = await GetDetailsOrThrowIfPaymentDoesNotExists(orderId);

            var cancelPaymentProcessResult = await _commandBus.PublishAsync(new CancelPaymentProcessCommand(PaymentId.With(paymentDetails.PaymentId)), CancellationToken.None);

            if (!cancelPaymentProcessResult.IsSuccess)
            {
                throw new InvalidOperationException("PaymentState cancellation process failed");
            }
        }

        public async Task CompletePaymentProcessAsync(Guid orderId)
        {
            var paymentDetails = await GetDetailsOrThrowIfPaymentDoesNotExists(orderId);

            var cancelPaymentProcessResult = await _commandBus.PublishAsync(new CompletePaymentProcessCommand(PaymentId.With(paymentDetails.PaymentId)), CancellationToken.None);
            if (!cancelPaymentProcessResult.IsSuccess)
            {
                throw new InvalidOperationException("PaymentState ping process failed");
            }
        }

        public async Task PingAsync(Guid orderId)
        {
            var paymentDetails = await GetDetailsOrThrowIfPaymentDoesNotExists(orderId);

            var cancelPaymentProcessResult = await _commandBus.PublishAsync(new PingPaymentProcessCommand(PaymentId.With(paymentDetails.PaymentId)), CancellationToken.None);
            if (!cancelPaymentProcessResult.IsSuccess)
            {
                throw new InvalidOperationException("PaymentState ping process failed");
            }
        }

        public async Task<PaymentDetailsReadModel> GetDetailsOrThrowIfPaymentDoesNotExists(Guid orderId)
        {
            var paymentDetails = await _queryProcessor.ProcessAsync(new GetPaymentDetailsQuery(orderId), CancellationToken.None)
                .ConfigureAwait(false);
            if (paymentDetails == null)
            {
                throw new InvalidOperationException($"Payment for order id: {orderId} does not exists!");
            }

            return paymentDetails;
        }
    }
}