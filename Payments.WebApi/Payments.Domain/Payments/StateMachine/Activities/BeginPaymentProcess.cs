using System.Threading.Tasks;
using Payments.Domain.Payments.StateMachine.Activities.Base;

namespace Payments.Domain.Payments.StateMachine.Activities
{
    public class BeginPaymentProcessData
    {
        public BeginPaymentProcessData(string country, string currency, string system, decimal amount, string externalId, string externalCallbackUrl)
        {
            Country = country;
            Currency = currency;
            System = system;
            Amount = amount;
            ExternalId = externalId;
            ExternalCallbackUrl = externalCallbackUrl;
        }

        public string Country { get; }
        public string Currency { get; }
        public string System { get; }
        public decimal Amount { get; }
        public string ExternalId { get; }
        public string ExternalCallbackUrl { get; }

    }

    public class BeginPaymentProcess : StateMachineActivity<PaymentState, BeginPaymentProcessData>
    {
        private readonly PaymentAggregate _paymentAggregate;

        public BeginPaymentProcess(PaymentAggregate paymentAggregate)
        {
            _paymentAggregate = paymentAggregate;
        }

        public override async Task Execute(BeginPaymentProcessData data)
        {
            await _paymentAggregate.BeginPaymentProcessAsync(data.Country, data.Currency,
                data.System, data.ExternalId, data.ExternalCallbackUrl, data.Amount);
        }
    }
}