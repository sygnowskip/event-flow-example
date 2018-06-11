using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using EventFlow.Commands;
using Payments.Domain.Orders.StateMachine;

namespace Payments.Domain.Orders.Commands
{
    public class ProcessToPaymentCommand : Command<OrderAggregate, OrderId>
    {
        public ProcessToPaymentCommand(OrderId aggregateId) : base(aggregateId)
        {
        }
    }

    public class ProcessToPaymentCommandHandler : CommandHandler<OrderAggregate, OrderId, ProcessToPaymentCommand>
    {
        public override async Task ExecuteAsync(OrderAggregate aggregate, ProcessToPaymentCommand command, CancellationToken cancellationToken)
        {
            var stateMachine = new OrderStateMachine();
            await stateMachine.RaiseEvent(aggregate, stateMachine.InitPaymentProcessRequested, cancellationToken);
        }
    }
}