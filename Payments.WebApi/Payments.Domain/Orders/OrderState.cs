using System;
using System.Collections.Generic;
using EventFlow.Aggregates;
using Payments.Domain.Orders.Events;

namespace Payments.Domain.Orders
{
    public enum OrderStatus
    {
        New = 3,
        PaymentInProgress,
        Completed,
        RequestingForPayment
    }

    public class OrderProduct
    {
        public OrderProduct(string name, int count, decimal price)
        {
            Name = name;
            Count = count;
            Price = price;
        }

        public string Name { get; }
        public int Count { get; }
        public decimal Price { get; }
    }

    public class OrderState : AggregateState<OrderAggregate, OrderId, OrderState>,
        IApply<OrderCreated>,
        IApply<ProductToOrderAdded>,
        IApply<OrderPaymentStarted>,
        IApply<OrderPaymentCompleted>,
        IApply<OrderPaymentFailed>,
        IApply<OrderPaymentRequested>
    {
        public OrderState()
        {
            Products = new List<OrderProduct>();
        }

        public OrderStatus Status { get; set; }
        public string Username { get; set; }
        public IList<OrderProduct> Products { get; set; }
        public Guid PaymentId { get; set; }

        public void Load(OrderState orderState)
        {
            Status = orderState.Status;
            Products = orderState.Products;
        }

        public void Apply(OrderCreated aggregateEvent)
        {
            Username = aggregateEvent.Username;
            Status = OrderStatus.New;
        }

        public void Apply(ProductToOrderAdded aggregateEvent)
        {
            Products.Add(new OrderProduct(aggregateEvent.Name, aggregateEvent.Count, aggregateEvent.Price));
        }

        public void Apply(OrderPaymentCompleted aggregateEvent)
        {
            Status = OrderStatus.Completed;
        }

        public void Apply(OrderPaymentFailed aggregateEvent)
        {
            Status = OrderStatus.New;
        }

        public void Apply(OrderPaymentStarted aggregateEvent)
        {
            PaymentId = aggregateEvent.PaymentId;
            Status = OrderStatus.PaymentInProgress;
        }

        public void Apply(OrderPaymentRequested aggregateEvent)
        {
            Status = OrderStatus.RequestingForPayment;
        }
    }
}