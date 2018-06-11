using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using EventFlow.Commands;
using Payments.Domain.Orders.StateMachine;

namespace Payments.Domain.Orders.Commands
{
    public class CreateOrderCommand : Command<OrderAggregate, OrderId>
    {
        public string Username { get; }
        public CreateOrderCommand(OrderId aggregateId, string username) : base(aggregateId)
        {
            Username = username;
        }
    }

    public class CreateOrderCommandHandler : CommandHandler<OrderAggregate, OrderId, CreateOrderCommand>
    {
        public override async Task ExecuteAsync(OrderAggregate aggregate, CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var stateMachine = new OrderStateMachine();
            await stateMachine.RaiseEvent(aggregate, stateMachine.OrderCreationRequested, command.Username, cancellationToken);
        }
    }
}