using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using EventFlow.Commands;
using Payments.Domain.Orders.StateMachine;
using Payments.Domain.Orders.StateMachine.Models;

namespace Payments.Domain.Orders.Commands
{
    public class AddProductToOrderCommand : Command<OrderAggregate, OrderId>
    {
        public string Name { get; }
        public int Count { get; }
        public decimal Price { get; }

        public AddProductToOrderCommand(OrderId aggregateId, string name, int count, decimal price) : base(aggregateId)
        {
            Name = name;
            Count = count;
            Price = price;
        }
    }

    public class AddProductToOrderCommandHandler : CommandHandler<OrderAggregate, OrderId, AddProductToOrderCommand>
    {
        public override async Task ExecuteAsync(OrderAggregate aggregate, AddProductToOrderCommand command, CancellationToken cancellationToken)
        {
            var stateMachine = new OrderStateMachine();
            await stateMachine.RaiseEvent(aggregate, stateMachine.AddProductToOrderRequested,
                new AddProductToOrderData(command.Name, command.Count, command.Price), cancellationToken);
        }
    }
}