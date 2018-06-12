using System;
using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using Payments.Domain.Payments.StateMachine;
using Payments.Domain.Payments.StateMachine.Models;

namespace Payments.Domain.Payments.Commands
{
    public class BeginPaymentProcessCommand : Command<PaymentAggregate, PaymentId, BeginPaymentProcessCommandResult>
    {
        public Guid OrderId { get; }
        public string Username { get; }
        public decimal TotalPrice { get; }

        public BeginPaymentProcessCommand(PaymentId aggregateId, Guid orderId, string username, decimal totalPrice) : base(aggregateId)
        {
            OrderId = orderId;
            Username = username;
            TotalPrice = totalPrice;
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
            var stateMachine = new PaymentStateMachine();
            await stateMachine.RaiseEvent(aggregate, stateMachine.PaymentInitiationRequested,
                new BeginPaymentProcessData(command.OrderId, command.Username, command.TotalPrice), cancellationToken);

            return new BeginPaymentProcessCommandResult(true, aggregate.PaymentState.RedirectUrl);
        }
    }
}