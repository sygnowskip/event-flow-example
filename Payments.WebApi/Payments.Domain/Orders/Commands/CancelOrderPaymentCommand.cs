using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using EventFlow.Commands;
using Payments.Domain.Orders.StateMachine;

namespace Payments.Domain.Orders.Commands
{
    public class CancelOrderPaymentCommand : Command<OrderAggregate, OrderId>
    {
        public CancelOrderPaymentCommand(OrderId aggregateId) : base(aggregateId)
        {
        }
    }

    public class CancelOrderPaymentCommandHandler : CommandHandler<OrderAggregate, OrderId, CancelOrderPaymentCommand>
    {
        public override async Task ExecuteAsync(OrderAggregate aggregate, CancelOrderPaymentCommand command, CancellationToken cancellationToken)
        {
            var stateMachine = new OrderStateMachine();
            await stateMachine.RaiseEvent(aggregate, stateMachine.PaymentProcessCanceled, cancellationToken);
        }
    }
}