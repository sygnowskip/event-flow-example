using EventFlow.Snapshots;
using Payments.Domain.Payments.Payments;

namespace Payments.Domain.Payments.Snapshots
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