using System;
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

namespace Payments.Domain.Orders
{
    public class OrderSagaLocator : ISagaLocator
    {
        public Task<ISagaId> LocateSagaAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var orderId = Guid.Empty;
            var aggregateEvent = domainEvent.GetAggregateEvent();
            if (aggregateEvent is PaymentProcessCompleted paymentProcessCompleted)
            {
                orderId = paymentProcessCompleted.OrderId;
            }
            else if (aggregateEvent is PaymentProcessCancelled paymentProcessCancelled)
            {
                orderId = paymentProcessCancelled.OrderId;
            }
            else if (domainEvent is IDomainEvent<OrderAggregate, OrderId, OrderPaymentStarted> orderPaymentStarted)
            {
                orderId = orderPaymentStarted.AggregateIdentity.GetGuid();
            }

            return Task.FromResult<ISagaId>(new OrderSagaId($"ordersaga-{orderId}"));
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