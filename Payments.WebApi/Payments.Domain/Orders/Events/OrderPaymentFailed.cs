using EventFlow.Aggregates;

namespace Payments.Domain.Orders.Events
{
    public class OrderPaymentFailed : AggregateEvent<OrderAggregate, OrderId>
    {
        
    }
}