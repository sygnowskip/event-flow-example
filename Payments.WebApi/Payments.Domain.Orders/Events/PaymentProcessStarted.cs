using EventFlow.Aggregates;

namespace Payments.Domain.Orders.Events
{
    public class PaymentProcessStarted : AggregateEvent<OrderAggregate, OrderId>
    {
        
    }
}