using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using EventFlow.Commands;
using Payments.Domain.Payments.StateMachine;

namespace Payments.Domain.Payments.Commands
{
    public class CompletePaymentProcessCommand : Command<PaymentAggregate, PaymentId>
    {
        public CompletePaymentProcessCommand(PaymentId aggregateId) : base(aggregateId)
        {
        }
    }

    public class CompletePaymentProcessCommandHandler : CommandHandler<PaymentAggregate, PaymentId, CompletePaymentProcessCommand>
    {
        public override async Task ExecuteAsync(PaymentAggregate aggregate, CompletePaymentProcessCommand command, CancellationToken cancellationToken)
        {
            var stateMachine = new PaymentStateMachine();
            await stateMachine.RaiseEvent(aggregate, stateMachine.PaymentCompletionRequested, cancellationToken);
        }
    }
}