using System;
using System.Threading.Tasks;

namespace Payments.Domain.Payments.Providers
{
    public interface IPaymentProvider
    {
        PaymentProviderType Type { get; }
        Task<Uri> BeginPaymentProcessAsync(BeginPaymentProcessModel request);
    }

    public class BeginPaymentProcessModel
    {
        public BeginPaymentProcessModel(Guid orderId, string username, decimal totalPrice)
        {
            OrderId = orderId;
            Username = username;
            TotalPrice = totalPrice;
        }

        public Guid OrderId { get; }
        public string Username { get; }
        public decimal TotalPrice { get; }
    }
}