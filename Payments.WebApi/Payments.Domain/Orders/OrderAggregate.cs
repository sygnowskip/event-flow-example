using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Aggregates;
using Payments.Domain.Common.Aggregate;
using Payments.Domain.Orders.Events;
using Payments.Domain.Payments;
using Payments.Domain.Payments.Commands;

namespace Payments.Domain.Orders
{
    public class OrderAggregate : AggregateRoot<OrderAggregate, OrderId>
    {
        private readonly ICommandBus _commandBus;
        public OrderState OrderState { get; } = new OrderState();

        public int StateMachineState
        {
            get => (int)OrderState.Status;
            set => OrderState.Status = (OrderStatus)value;
        }

        public OrderAggregate(OrderId id, ICommandBus commandBus) : base(id)
        {
            _commandBus = commandBus;
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

        public async Task BeginPaymentProcess()
        {
            var totalPrice = OrderState.Products.Sum(p => p.Count * p.Price);
            await _commandBus.PublishAsync(new BeginPaymentProcessCommand(PaymentId.New, Id.GetGuid(),
                OrderState.Username, totalPrice), CancellationToken.None);
            Emit(new OrderPaymentStarted(), this.MetadataFor(new { OrderId = Id.GetGuid() }));
        }

        public void MarkPaymentProcessAsFailed()
        {
            Emit(new OrderPaymentFailed());
        }
    }
}