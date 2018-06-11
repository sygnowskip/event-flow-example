using System;
using System.Threading.Tasks;

namespace Payments.Domain.Payments.Providers.Types
{
    public class TestProvider2PaymentProvider : IPaymentProvider
    {
        public PaymentProviderType Type { get; } = PaymentProviderType.TestProvider2;
        public Task<Uri> BeginPaymentProcessAsync(BeginPaymentProcessModel request)
        {
            //call TestProvider2 api, do anything you need to
            return Task.FromResult(new Uri("http://jpm/pay"));
        }
    }
}