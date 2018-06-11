using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using Payments.Domain.Orders;
using Payments.Domain.Orders.Commands;

namespace Payments.Application.Payments
{
    public interface IOrdersApplicationService
    {
        Task<Guid> CreateOrder(string username);
        Task AddProduct(Guid orderId, string name, int count, decimal amount);
        Task ProcessToPayment(Guid orderId);
    }

    public class OrdersApplicationService : IOrdersApplicationService
    {
        private readonly ICommandBus _commandBus;

        public OrdersApplicationService(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public async Task<Guid> CreateOrder(string username)
        {
            var orderId = OrderId.New;
            await _commandBus.PublishAsync(new CreateOrderCommand(orderId, username), CancellationToken.None);
            return orderId.GetGuid();
        }

        public async Task AddProduct(Guid orderId, string name, int count, decimal price)
        {
            await _commandBus.PublishAsync(new AddProductToOrderCommand(OrderId.With(orderId), name, count, price), CancellationToken.None);
        }

        public async Task ProcessToPayment(Guid orderId)
        {
            await _commandBus.PublishAsync(new ProcessToPaymentCommand(OrderId.With(orderId)), CancellationToken.None);
        }
    }
}