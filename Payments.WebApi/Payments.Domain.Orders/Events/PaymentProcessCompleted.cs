using EventFlow.Aggregates;

namespace Payments.Domain.Orders.Events
{
    public class PaymentProcessCompleted : AggregateEvent<OrderAggregate, OrderId>
    {
        
    }
}