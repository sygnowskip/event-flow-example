using Payments.Domain.Payments.Providers;

namespace Payments.Domain.Payments.Configuration
{
    public interface IConfigurationProvider
    {
        PaymentProviderType GetPaymentProviderType(string country, string system, string currency);
    }

    public class ConfigurationProvider : IConfigurationProvider
    {
        public PaymentProviderType GetPaymentProviderType(string country, string system, string currency)
        {
            //TODO: Read it from read models
            return country.Length % 2 == 0 ? PaymentProviderType.TestProvider1 : PaymentProviderType.TestProvider2;
        }
    }
}