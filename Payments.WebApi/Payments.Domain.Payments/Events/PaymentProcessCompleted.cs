using EventFlow.Aggregates;
using Payments.Domain.Payments.Payments;

namespace Payments.Domain.Payments.Events
{
    public class PaymentProcessCompleted : AggregateEvent<PaymentAggregate, PaymentId>
    {
        
    }
}