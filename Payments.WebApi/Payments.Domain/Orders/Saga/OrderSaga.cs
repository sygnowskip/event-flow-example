using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Sagas;
using EventFlow.Sagas.AggregateSagas;
using EventFlow.ValueObjects;
using Payments.Domain.Orders.Commands;
using Payments.Domain.Orders.Events;
using Payments.Domain.Orders.Saga.Events;
using Payments.Domain.Payments;
using Payments.Domain.Payments.Events;
using Payments.Domain.Common.Saga;

namespace Payments.Domain.Orders
{
    public class OrderSagaLocator : BaseIdSagaLocator
    {
        
        public OrderSagaLocator() : base(nameof(OrderId), id => new OrderSagaId($"ordersaga-{id}"))
        {
        }
    }

    public class OrderSagaId : SingleValueObject<string>, ISagaId
    {
        public OrderSagaId(string value) : base(value)
        {
        }
    }

    public class OrderSaga : AggregateSaga<OrderSaga, OrderSagaId, OrderSagaLocator>,
        ISagaIsStartedBy<OrderAggregate, OrderId, OrderPaymentStarted>,
        ISagaHandles<PaymentAggregate, PaymentId, PaymentProcessCompleted>,
        ISagaHandles<PaymentAggregate, PaymentId, PaymentProcessCancelled>,
        IEmit<OrderSagaStarted>
    {
        private OrderId OrderId { get; set; }
        public OrderSaga(OrderSagaId id) : base(id)
        {
        }

        public Task HandleAsync(IDomainEvent<PaymentAggregate, PaymentId, PaymentProcessCompleted> domainEvent, ISagaContext sagaContext, CancellationToken cancellationToken)
        {
            Publish(new CompleteOrderCommand(OrderId.With(domainEvent.AggregateEvent.OrderId)));
            return Task.CompletedTask;
        }

        public Task HandleAsync(IDomainEvent<PaymentAggregate, PaymentId, PaymentProcessCancelled> domainEvent, ISagaContext sagaContext, CancellationToken cancellationToken)
        {
            Publish(new CancelOrderPaymentCommand(OrderId.With(domainEvent.AggregateEvent.OrderId)));
            return Task.CompletedTask;
        }

        public Task HandleAsync(IDomainEvent<OrderAggregate, OrderId, OrderPaymentStarted> domainEvent, ISagaContext sagaContext, CancellationToken cancellationToken)
        {
            Emit(new OrderSagaStarted(domainEvent.AggregateIdentity));
            return Task.CompletedTask;
        }

        public void Apply(OrderSagaStarted aggregateEvent)
        {
            OrderId = aggregateEvent.OrderId;
        }
    }
}