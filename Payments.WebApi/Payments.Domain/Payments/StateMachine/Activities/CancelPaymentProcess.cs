using System.Threading.Tasks;
using Payments.Domain.Payments.StateMachine.Activities.Base;

namespace Payments.Domain.Payments.StateMachine.Activities
{
    public class CancelPaymentProcess : StateMachineActivity<PaymentState>
    {
        private readonly PaymentAggregate _paymentAggregate;

        public CancelPaymentProcess(PaymentAggregate paymentAggregate)
        {
            _paymentAggregate = paymentAggregate;
        }

        public override Task Execute()
        {
            _paymentAggregate.CancelPaymentProcess();
            return Task.CompletedTask;
        }
    }
}