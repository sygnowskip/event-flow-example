using EventFlow.Aggregates;

namespace Payments.Domain.Payments.Payments.Events
{
    public class PaymentProcessPinged : AggregateEvent<PaymentAggregate, PaymentId>
    {
        
    }
}