using EventFlow.Aggregates;

namespace Payments.Domain.Orders.Events
{
    public class OrderCreated : AggregateEvent<OrderAggregate, OrderId>
    {
        public OrderCreated(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}