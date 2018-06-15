using EventFlow.Aggregates;
using Payments.Domain.Payments;

namespace Payments.Domain.Orders.Saga.Events
{
    public class OrderPaymentRequestCompleted : AggregateEvent<OrderSaga, OrderSagaId>
    {
        public OrderPaymentRequestCompleted(OrderId orderId, PaymentId paymentId, decimal amount)
        {
            OrderId = orderId;
            Amount = amount;
            PaymentId = paymentId;
        }

        public OrderId OrderId { get; }
        public decimal Amount { get; }
        public PaymentId PaymentId { get; }
    }
}