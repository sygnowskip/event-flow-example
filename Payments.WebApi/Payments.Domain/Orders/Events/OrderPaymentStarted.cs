using EventFlow.Aggregates;

namespace Payments.Domain.Orders.Events
{
    public class OrderPaymentStarted : AggregateEvent<OrderAggregate, OrderId>
    {
        
    }
}