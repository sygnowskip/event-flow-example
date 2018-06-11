using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using EventFlow.Commands;
using Payments.Domain.Payments.StateMachine;

namespace Payments.Domain.Payments.Commands
{
    public class CancelPaymentProcessCommand : Command<PaymentAggregate, PaymentId>
    {
        public CancelPaymentProcessCommand(PaymentId aggregateId) : base(aggregateId)
        {
        }
    }

    public class CancelPaymentProcessCommandHandler : CommandHandler<PaymentAggregate, PaymentId, CancelPaymentProcessCommand>
    {
        public override async Task ExecuteAsync(PaymentAggregate aggregate, CancelPaymentProcessCommand command, CancellationToken cancellationToken)
        {
            var stateMachine = new PaymentStateMachine();
            await stateMachine.RaiseEvent(aggregate, stateMachine.PaymentCancellationRequested, cancellationToken);
        }
    }
}