using EventFlow.Snapshots;

namespace Payments.Domain.Payments.Payments.Snapshots
{

    public class PaymentAggregateSnapshot : ISnapshot
    {
        public PaymentAggregateSnapshot(PaymentState paymentState)
        {
            PaymentState = paymentState;
        }

        public PaymentState PaymentState { get; }
    }
}