using System;
using System.Threading.Tasks;

namespace Payments.Domain.Providers.Types
{
    public class AdyenPaymentProvider : IPaymentProvider
    {
        public PaymentProviderType Type { get; } = PaymentProviderType.Adyen;
        public Task<Uri> BeginPaymentProcessAsync(BeginPaymentProcessModel request)
        {
            //call JPM api, do anything you need to
            return Task.FromResult(new Uri("http://adyen/pay"));
        }
    }
}