using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

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
        public override Task ExecuteAsync(PaymentAggregate aggregate, CancelPaymentProcessCommand command, CancellationToken cancellationToken)
        {
            aggregate.CancelPaymentProcess();
            return Task.CompletedTask;
        }
    }
}