using System.Threading.Tasks;
using EventFlow.Aggregates;

namespace Payments.Domain.Payments
{
    public class PaymentAggregate : AggregateRoot<PaymentAggregate, PaymentId>
    {
        private readonly PaymentState _paymentState = new PaymentState();
        
        public PaymentAggregate(PaymentId id) : base(id)
        {
            Register(_paymentState);
        }

        public async Task<string> BeginPaymentProcessAsync(string country, string currency, string system, string externalId,
            decimal amount)
        {
            return Task.FromResult(string.Empty);
        }
    }
}