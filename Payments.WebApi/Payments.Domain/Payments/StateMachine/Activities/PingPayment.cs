using System.Threading.Tasks;
using Payments.Domain.Payments.StateMachine.Activities.Base;

namespace Payments.Domain.Payments.StateMachine.Activities
{
    public class PingPayment : StateMachineActivity<PaymentState>
    {
        private readonly PaymentAggregate _paymentAggregate;

        public PingPayment(PaymentAggregate paymentAggregate)
        {
            _paymentAggregate = paymentAggregate;
        }

        public override Task Execute()
        {
            _paymentAggregate.Ping();
            return Task.CompletedTask;
        }
    }
}