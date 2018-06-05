using Automatonymous;
using Payments.Domain.Payments.StateMachine.Activities;

namespace Payments.Domain.Payments
{
    public class PaymentStateMachine : AutomatonymousStateMachine<PaymentState>
    {
        public PaymentStateMachine(PaymentAggregate paymentAggregate)
        {
            InstanceState(x => x.StateMachineState, Started, Cancelled);

            Initially(
                When(PaymentInitiationRequested)
                    .Execute(context => new BeginPaymentProcess(paymentAggregate))
                    .TransitionTo(Started));

            During(Started,
                When(PaymentPingRequested)
                    .Execute(context => new PingPayment(paymentAggregate)),
                When(PaymentCancellationRequested)
                    .Execute(context => new CancelPaymentProcess(paymentAggregate))
                    .TransitionTo(Cancelled)
                    .Finalize());
        }

        public Event<BeginPaymentProcessData> PaymentInitiationRequested { get; private set; }
        public Event PaymentCancellationRequested { get; private set; }
        public Event PaymentPingRequested { get; private set; }

        public State Started { get; private set; }
        public State Cancelled { get; private set; }
    }
}