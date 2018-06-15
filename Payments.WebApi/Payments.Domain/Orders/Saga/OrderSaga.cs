using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Sagas;
using EventFlow.Sagas.AggregateSagas;
using EventFlow.ValueObjects;
using Payments.Domain.Common.Saga;
using Payments.Domain.Orders.Commands;
using Payments.Domain.Orders.Events;
using Payments.Domain.Orders.Saga.Events;
using Payments.Domain.Payments;
using Payments.Domain.Payments.Commands;
using Payments.Domain.Payments.Events;

namespace Payments.Domain.Orders.Saga
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
        ISagaIsStartedBy<OrderAggregate, OrderId, OrderPaymentRequested>,
        ISagaHandles<PaymentAggregate, PaymentId, PaymentProcessCompleted>,
        ISagaHandles<PaymentAggregate, PaymentId, PaymentProcessCancelled>,
        IEmit<OrderPaymentRequestCompleted>
    {
        private OrderId OrderId { get; set; }
        private PaymentId PaymentId { get; set; }
        private decimal TotalAmount { get; set; }
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

        public Task HandleAsync(IDomainEvent<OrderAggregate, OrderId, OrderPaymentRequested> domainEvent, ISagaContext sagaContext, CancellationToken cancellationToken)
        {
            var paymentId = PaymentId.New;
            Publish(new BeginPaymentProcessCommand(paymentId, domainEvent.AggregateIdentity.GetGuid(),
                 domainEvent.AggregateEvent.Username, domainEvent.AggregateEvent.Amount));
            
            Emit(new OrderPaymentRequestCompleted(domainEvent.AggregateIdentity, paymentId, domainEvent.AggregateEvent.Amount));

            Publish(new MarkPaymentProcessAsStartedCommand(domainEvent.AggregateIdentity, paymentId.GetGuid()));

            return Task.CompletedTask;
        }

        public void Apply(OrderPaymentRequestCompleted aggregateEvent)
        {
            OrderId = aggregateEvent.OrderId;
            PaymentId = aggregateEvent.PaymentId;
            TotalAmount = aggregateEvent.Amount;
        }
    }
}