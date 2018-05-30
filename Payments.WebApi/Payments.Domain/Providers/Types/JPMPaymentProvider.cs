using System;
using System.Threading.Tasks;

namespace Payments.Domain.Providers.Types
{
    public class JPMPaymentProvider : IPaymentProvider
    {
        public PaymentProviderType Type { get; } = PaymentProviderType.JPM;
        public Task<Uri> BeginPaymentProcessAsync(BeginPaymentProcessModel request)
        {
            //call JPM api, do anything you need to
            return Task.FromResult(new Uri("http://jpm/pay"));
        }
    }
}