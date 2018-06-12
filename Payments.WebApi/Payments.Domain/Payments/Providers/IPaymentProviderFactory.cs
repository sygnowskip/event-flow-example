using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;


namespace Payments.Domain.Payments.Providers
{
    public interface IPaymentProviderFactory
    {
        IPaymentProvider GetPaymentProvider();
    }

    public class PaymentProviderFactory : IPaymentProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfigurationProvider _configurationProvider;

        public PaymentProviderFactory(IConfigurationProvider configurationProvider, IServiceProvider serviceProvider)
        {
            _configurationProvider = configurationProvider;
            _serviceProvider = serviceProvider;
        }

        public IPaymentProvider GetPaymentProvider()
        {
            var paymentProviderType = _configurationProvider.GetPaymentProviderType();
            var paymentProvider = _serviceProvider.GetServices<IPaymentProvider>().SingleOrDefault(p => p.Type == paymentProviderType);

            if (paymentProvider == null)
            {
                throw new InvalidOperationException($"Cannot find a paymentState provider for {paymentProviderType}");
            }
            return paymentProvider;
        }
    }
}