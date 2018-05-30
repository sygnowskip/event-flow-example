using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Payments.Domain.Configuration;

namespace Payments.Domain.Providers
{
    public interface IPaymentProviderFactory
    {
        IPaymentProvider GetPaymentProvider(string country, string system, string currency);
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

        public IPaymentProvider GetPaymentProvider(string country, string system, string currency)
        {
            var paymentProviderType = _configurationProvider.GetPaymentProviderType(country, system, currency);
            var paymentProvider = _serviceProvider.GetServices<IPaymentProvider>().SingleOrDefault(p => p.Type == paymentProviderType);

            if (paymentProvider == null)
            {
                throw new InvalidOperationException($"Cannot find a payment provider for {paymentProviderType}");
            }
            return paymentProvider;
        }
    }
}