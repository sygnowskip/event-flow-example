using EventFlow.Aggregates;

namespace Payments.Domain.Payments.Events
{
    public class PaymentProcessPinged : AggregateEvent<PaymentAggregate, PaymentId>
    {
        
    }
}