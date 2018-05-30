using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace Payments.Domain.Payments.Commands
{
    public class BeginPaymentProcessCommand : Command<PaymentAggregate, PaymentId, BeginPaymentProcessCommandResult>
    {
        public string Country { get; }
        public string Currency { get; }
        public string System { get; }
        public string ExternalId { get; }
        public string ExternalCallbackUrl { get; }
        public decimal Amount { get; }

        public BeginPaymentProcessCommand(PaymentId aggregateId, string country, string currency, string system, string externalId, string externalCallbackUrl, decimal amount) : base(aggregateId)
        {
            Country = country;
            Currency = currency;
            System = system;
            ExternalId = externalId;
            Amount = amount;
            ExternalCallbackUrl = externalCallbackUrl;
        }
    }

    public class BeginPaymentProcessCommandResult : ExecutionResult
    {
        public BeginPaymentProcessCommandResult(bool isSuccess, Uri redirectUrl)
        {
            IsSuccess = isSuccess;
            RedirectUrl = redirectUrl;
        }

        public override bool IsSuccess { get; }
        public Uri RedirectUrl { get; }
    }

    public class
        BeginPaymentProcessCommandHandler : CommandHandler<PaymentAggregate, PaymentId, BeginPaymentProcessCommandResult, BeginPaymentProcessCommand>
    {
        public override async Task<BeginPaymentProcessCommandResult> ExecuteCommandAsync(PaymentAggregate aggregate, BeginPaymentProcessCommand command, CancellationToken cancellationToken)
        {
            var redirectUrl = await aggregate.BeginPaymentProcessAsync(command.Country, command.Currency, command.System, command.ExternalId, command.ExternalCallbackUrl,
                command.Amount);
            return new BeginPaymentProcessCommandResult(true, redirectUrl);
        }
    }
}