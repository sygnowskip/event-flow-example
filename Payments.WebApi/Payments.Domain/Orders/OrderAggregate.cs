using System;
using System.Linq;
using EventFlow;
using EventFlow.Aggregates;
using Payments.Domain.Common.Aggregate;
using Payments.Domain.Orders.Events;

namespace Payments.Domain.Orders
{
    public class OrderAggregate : AggregateRoot<OrderAggregate, OrderId>
    {
        public OrderState OrderState { get; } = new OrderState();

        public int StateMachineState
        {
            get => (int)OrderState.Status;
            set => OrderState.Status = (OrderStatus)value;
        }

        public OrderAggregate(OrderId id, ICommandBus commandBus) : base(id)
        {
            Register(OrderState);
        }

        public void CompletePaymentProcess()
        {
            Emit(new OrderPaymentCompleted());
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
            var totalPrice = OrderState.Products.Sum(p => p.Count * p.Price);
            Emit(new OrderPaymentRequested(totalPrice, OrderState.Username), this.MetadataFor(new { OrderId = Id.GetGuid() }));
        }

        public void MarkPaymentProcessAsFailed()
        {
            Emit(new OrderPaymentFailed());
        }

        public void MarkPaymentProcessAsStarted(Guid paymentId)
        {
            Emit(new OrderPaymentStarted(paymentId));
        }
    }
}