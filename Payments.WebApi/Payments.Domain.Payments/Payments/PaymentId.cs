using EventFlow.Core;

namespace Payments.Domain.Payments.Payments
{
    public class PaymentId : Identity<PaymentId>
    {
        public PaymentId(string value) : base(value)
        {
        }
    }
}