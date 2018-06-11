using EventFlow.Aggregates;

namespace Payments.Domain.Orders.Events
{
    public class PaymentProcessFailed : AggregateEvent<OrderAggregate, OrderId>
    {
        
    }
}