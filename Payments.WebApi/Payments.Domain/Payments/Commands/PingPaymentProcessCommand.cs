using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using EventFlow.Commands;

namespace Payments.Domain.Payments.Commands
{
    public class PingPaymentProcessCommand : Command<PaymentAggregate, PaymentId>
    {
        public PingPaymentProcessCommand(PaymentId aggregateId) : base(aggregateId)
        {
        }
    }

    public class PingPaymentProcessCommandHandler : CommandHandler<PaymentAggregate, PaymentId, PingPaymentProcessCommand>
    {
        public override async Task ExecuteAsync(PaymentAggregate aggregate, PingPaymentProcessCommand command, CancellationToken cancellationToken)
        {
            var stateMachine = new PaymentStateMachine(aggregate);
            await stateMachine.RaiseEvent(aggregate.PaymentState, stateMachine.PaymentPingRequested, cancellationToken);
        }
    }
}