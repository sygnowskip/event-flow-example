using System;
using EventFlow.Core;

namespace Payments.Domain.Payments
{
    public class PaymentId : Identity<PaymentId>
    {
        public PaymentId() : base(Guid.NewGuid().ToString())
        {
        }
    }
}