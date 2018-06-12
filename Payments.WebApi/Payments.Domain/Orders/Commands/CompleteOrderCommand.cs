using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using EventFlow.Commands;
using Payments.Domain.Orders.StateMachine;

namespace Payments.Domain.Orders.Commands
{
    public class CompleteOrderCommand : Command<OrderAggregate, OrderId>
    {
        public CompleteOrderCommand(OrderId aggregateId) : base(aggregateId)
        {
        }
    }

    public class CompleteOrderCommandHandler : CommandHandler<OrderAggregate, OrderId, CompleteOrderCommand>
    {
        public override async Task ExecuteAsync(OrderAggregate aggregate, CompleteOrderCommand command, CancellationToken cancellationToken)
        {
            var stateMachine = new OrderStateMachine();
            await stateMachine.RaiseEvent(aggregate, stateMachine.PaymentProcessSuccessfullyFinished, cancellationToken);
        }
    }
}