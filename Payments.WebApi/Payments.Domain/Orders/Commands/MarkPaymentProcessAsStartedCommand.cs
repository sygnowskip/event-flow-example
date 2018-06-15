using System;
using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using EventFlow.Commands;
using Payments.Domain.Orders.StateMachine;

namespace Payments.Domain.Orders.Commands
{
    public class MarkPaymentProcessAsStartedCommand : Command<OrderAggregate, OrderId>
    {
        public Guid PaymentId { get; }
        public MarkPaymentProcessAsStartedCommand(OrderId aggregateId, Guid paymentId) : base(aggregateId)
        {
            PaymentId = paymentId;
        }
    }

    public class MarkPaymentProcessAsStartedCommandHandler : CommandHandler<OrderAggregate, OrderId, MarkPaymentProcessAsStartedCommand>
    {
        public override async Task ExecuteAsync(OrderAggregate aggregate, MarkPaymentProcessAsStartedCommand command,
            CancellationToken cancellationToken)
        {
            var stateMachine = new OrderStateMachine();
            await stateMachine.RaiseEvent(aggregate, stateMachine.PaymentProcessSuccessfullyRequested, command.PaymentId, cancellationToken);
        }
    }
}