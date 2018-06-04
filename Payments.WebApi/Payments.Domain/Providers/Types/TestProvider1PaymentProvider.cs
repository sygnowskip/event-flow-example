using System;
using System.Threading.Tasks;

namespace Payments.Domain.Providers.Types
{
    public class TestProvider1PaymentProvider : IPaymentProvider
    {
        public PaymentProviderType Type { get; } = PaymentProviderType.TestProvider1;
        public Task<Uri> BeginPaymentProcessAsync(BeginPaymentProcessModel request)
        {
            //call TestProvider1 api, do anything you need to
            return Task.FromResult(new Uri("http://adyen/pay"));
        }
    }
}