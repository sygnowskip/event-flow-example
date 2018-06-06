using System.Threading.Tasks;
using Automatonymous;
using Payments.Domain.Payments.StateMachine.Models;

namespace Payments.Domain.Payments.StateMachine
{
    public class PaymentStateMachine : AutomatonymousStateMachine<PaymentAggregate>
    {
        public PaymentStateMachine()
        {
            InstanceState(x => x.StateMachineState, Started, Cancelled);

            Initially(
                When(PaymentInitiationRequested)
                    .ThenAsync(BeginPaymentProcesss)
                    .TransitionTo(Started));

            During(Started,
                When(PaymentPingRequested)
                    .Then(PingPayment),
                When(PaymentCancellationRequested)
                    .Then(CancelPaymentProcess)
                    .TransitionTo(Cancelled)
                    .Finalize());
        }

        private async Task BeginPaymentProcesss(BehaviorContext<PaymentAggregate, BeginPaymentProcessData> ctx)
        {
            await ctx.Instance.BeginPaymentProcessAsync(ctx.Data.Country, ctx.Data.Currency,
                ctx.Data.System, ctx.Data.ExternalId, ctx.Data.ExternalCallbackUrl, ctx.Data.Amount);
        }

        private void PingPayment(BehaviorContext<PaymentAggregate> context)
        {
            context.Instance.Ping();
        }

        private void CancelPaymentProcess(BehaviorContext<PaymentAggregate> context)
        {
            context.Instance.CancelPaymentProcess();
        }

        public Event<BeginPaymentProcessData> PaymentInitiationRequested { get; private set; }
        public Event PaymentCancellationRequested { get; private set; }
        public Event PaymentPingRequested { get; private set; }

        public State Started { get; private set; }
        public State Cancelled { get; private set; }
    }
}