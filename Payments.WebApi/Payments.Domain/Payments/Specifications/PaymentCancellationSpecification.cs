using System.Collections.Generic;
using EventFlow.Specifications;

namespace Payments.Domain.Payments.Specifications
{
    public class PaymentCancellationSpecification : Specification<PaymentAggregate>
    {
        protected override IEnumerable<string> IsNotSatisfiedBecause(PaymentAggregate aggregate)
        {
            if (aggregate.PaymentStatus != PaymentStatus.Started)
            {
                yield return $"Payment cannot be cancelled";
            }
        }
    }
}