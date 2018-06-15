using EventFlow.Aggregates;

namespace Payments.Domain.Orders.Events
{
    public class OrderPaymentRequested : AggregateEvent<OrderAggregate, OrderId>
    {
        public OrderPaymentRequested(decimal amount, string username)
        {
            Amount = amount;
            Username = username;
        }

        public decimal Amount { get; }
        public string Username { get; }
    }
}