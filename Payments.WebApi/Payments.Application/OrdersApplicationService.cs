using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using Payments.Domain.Orders.Commands;
using Payments.Domain.Orders;

namespace Payments.Application
{
    public interface IOrdersApplicationService
    {
        Task<Guid> CreateOrderAsync(string username);
        Task AddProductAsync(Guid orderId, string name, int count, decimal amount);
        Task ProcessToPaymentAsync(Guid orderId);
    }

    public class OrdersApplicationService : IOrdersApplicationService
    {
        private readonly ICommandBus _commandBus;

        public OrdersApplicationService(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public async Task<Guid> CreateOrderAsync(string username)
        {
            var orderId = OrderId.New;
            await _commandBus.PublishAsync(new CreateOrderCommand(orderId, username), CancellationToken.None);
            return orderId.GetGuid();
        }

        public async Task AddProductAsync(Guid orderId, string name, int count, decimal price)
        {
            await _commandBus.PublishAsync(new AddProductToOrderCommand(OrderId.With(orderId), name, count, price), CancellationToken.None);
        }

        public async Task ProcessToPaymentAsync(Guid orderId)
        {
            await _commandBus.PublishAsync(new ProcessToPaymentCommand(OrderId.With(orderId)), CancellationToken.None);
        }
    }
}