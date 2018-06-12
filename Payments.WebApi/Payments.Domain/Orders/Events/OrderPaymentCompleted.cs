using EventFlow.Aggregates;

namespace Payments.Domain.Orders.Events
{
    public class OrderPaymentCompleted : AggregateEvent<OrderAggregate, OrderId>
    {
        
    }
}