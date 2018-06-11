using EventFlow.Aggregates;

namespace Payments.Domain.Payments.Events
{
    public class PaymentProcessCompleted : AggregateEvent<PaymentAggregate, PaymentId>
    {
        
    }
}