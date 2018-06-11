using EventFlow.Aggregates;
using Payments.Domain.Orders.Events;

namespace Payments.Domain.Orders
{
    public class OrderAggregate : AggregateRoot<OrderAggregate, OrderId>
    { 
        public OrderState OrderState { get; }= new OrderState();

        public int StateMachineState
        {
            get => (int)OrderState.Status;
            set => OrderState.Status = (OrderStatus)value;
        }

        public OrderAggregate(OrderId id) : base(id)
        {
            Register(OrderState);
        }

        public void CompletePaymentProcess()
        {
            Emit(new PaymentProcessCompleted());
        }

        public void CreateOrder(string username)
        {
            Emit(new OrderCreated(username));
        }

        public void AddProductToOrder(string name, int count, decimal price)
        {
            Emit(new ProductToOrderAdded(name, count, price));
        }

        public void BeginPaymentProcess()
        {
            Emit(new PaymentProcessStarted());
        }

        public void MarkPaymentProcessAsFailed()
        {
            Emit(new PaymentProcessFailed());
        }
    }
}