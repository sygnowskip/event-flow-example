using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using EventFlow.Commands;
using Payments.Domain.Payments.Payments;
using Payments.Domain.Payments.StateMachine;

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
            var stateMachine = new PaymentStateMachine();
            await stateMachine.RaiseEvent(aggregate, stateMachine.PaymentPingRequested, cancellationToken);
        }
    }
}