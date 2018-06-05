using System;
using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using Payments.Domain.Payments.StateMachine.Activities;

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
            var stateMachine = new PaymentStateMachine(aggregate);
            await stateMachine.RaiseEvent(aggregate.PaymentState, stateMachine.PaymentInitiationRequested,
                new BeginPaymentProcessData(command.Country, command.Currency, command.System, command.Amount, command.ExternalId, command.ExternalCallbackUrl), cancellationToken);

            return new BeginPaymentProcessCommandResult(true, aggregate.PaymentState.RedirectUrl);
        }
    }
}