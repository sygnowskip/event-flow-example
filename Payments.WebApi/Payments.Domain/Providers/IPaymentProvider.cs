using System;
using System.Threading.Tasks;

namespace Payments.Domain.Providers
{
    public interface IPaymentProvider
    {
        PaymentProviderType Type { get; }
        Task<Uri> BeginPaymentProcessAsync(BeginPaymentProcessModel request);
    }

    public class BeginPaymentProcessModel
    {
        public BeginPaymentProcessModel(string paymentId, string country, string currency, string system, decimal amount)
        {
            PaymentId = paymentId;
            Country = country;
            Currency = currency;
            System = system;
            Amount = amount;
        }

        public string PaymentId { get; }
        public string Country { get; }
        public string Currency { get; }
        public string System { get; }
        public decimal Amount { get; }
    }
}