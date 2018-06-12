using EventFlow.Aggregates;

namespace Payments.Domain.Orders.Saga.Events
{
    public class OrderSagaStarted : AggregateEvent<OrderSaga, OrderSagaId>
    {
        public OrderSagaStarted(OrderId orderId)
        {
            OrderId = orderId;
        }

        public OrderId OrderId { get; }
    }
}