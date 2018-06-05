using Automatonymous;
using EventFlow.Aggregates;

namespace Payments.Domain.Payments.Events
{
    public class PaymentProcessCancelled : AggregateEvent<PaymentAggregate, PaymentId>
    {
        public PaymentProcessCancelled(string machineState)
        {
            MachineState = machineState;
        }

        public PaymentStatus Status { get; } = PaymentStatus.Cancelled;
        public string MachineState { get; }
    }
}