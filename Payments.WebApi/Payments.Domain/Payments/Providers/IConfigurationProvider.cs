namespace Payments.Domain.Payments.Providers
{
    public interface IConfigurationProvider
    {
        PaymentProviderType GetPaymentProviderType();
    }

    public class ConfigurationProvider : IConfigurationProvider
    {
        public PaymentProviderType GetPaymentProviderType()
        {
            //TODO: Read it from read models
            return PaymentProviderType.TestProvider1;
        }
    }
}