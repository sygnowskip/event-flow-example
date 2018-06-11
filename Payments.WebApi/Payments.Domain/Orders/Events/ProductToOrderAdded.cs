using EventFlow.Aggregates;

namespace Payments.Domain.Orders.Events
{
    public class ProductToOrderAdded : AggregateEvent<OrderAggregate, OrderId>
    {
        public ProductToOrderAdded(string name, int count, decimal price)
        {
            Name = name;
            Count = count;
            Price = price;
        }

        public string Name { get; }
        public int Count { get; }
        public decimal Price { get; }
    }
}