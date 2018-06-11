using EventFlow.Aggregates;
using Payments.Domain.Payments.Payments;

namespace Payments.Domain.Payments.Events
{
    public class PaymentProcessPinged : AggregateEvent<PaymentAggregate, PaymentId>
    {
        
    }
}