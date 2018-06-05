using System;
using System.Threading.Tasks;
using Automatonymous;
using GreenPipes;

namespace Payments.Domain.Payments
{
    public class PingPayment : Activity<PaymentState>
    {
        private readonly PaymentAggregate _paymentAggregate;

        public PingPayment(PaymentAggregate paymentAggregate)
        {
            _paymentAggregate = paymentAggregate;
        }

        public void Probe(ProbeContext context)
        {
        }

        public void Accept(StateMachineVisitor visitor)
        {
        }

        public Task Execute(BehaviorContext<PaymentState> context, Behavior<PaymentState> next)
        {
            _paymentAggregate.PingInternal();
            return Task.CompletedTask;
        }

        public Task Execute<T>(BehaviorContext<PaymentState, T> context, Behavior<PaymentState, T> next)
        {
            _paymentAggregate.PingInternal();
            return Task.CompletedTask;
        }

        public Task Faulted<TException>(BehaviorExceptionContext<PaymentState, TException> context, Behavior<PaymentState> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public Task Faulted<T, TException>(BehaviorExceptionContext<PaymentState, T, TException> context, Behavior<PaymentState, T> next) where TException : Exception
        {
            return next.Faulted(context);
        }
    }

    public class CancelPaymentProcess : Activity<PaymentState>
    {
        private readonly PaymentAggregate _paymentAggregate;

        public CancelPaymentProcess(PaymentAggregate paymentAggregate)
        {
            _paymentAggregate = paymentAggregate;
        }

        public void Probe(ProbeContext context)
        {
        }

        public void Accept(StateMachineVisitor visitor)
        {
        }

        public Task Execute(BehaviorContext<PaymentState> context, Behavior<PaymentState> next)
        {
            _paymentAggregate.CancelPaymentProcessAsyncInternal();
            return next.Execute(context);
        }

        public Task Execute<T>(BehaviorContext<PaymentState, T> context, Behavior<PaymentState, T> next)
        {
            _paymentAggregate.CancelPaymentProcessAsyncInternal();
            return next.Execute(context);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<PaymentState, TException> context, Behavior<PaymentState> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public Task Faulted<T, TException>(BehaviorExceptionContext<PaymentState, T, TException> context, Behavior<PaymentState, T> next) where TException : Exception
        {
            return next.Faulted(context);
        }
    }

    public class BeginPaymentProcess : Activity<PaymentState, PaymentInitiatedEvent>
    {
        private readonly PaymentAggregate _paymentAggregate;

        public BeginPaymentProcess(PaymentAggregate paymentAggregate)
        {
            _paymentAggregate = paymentAggregate;
        }

        public void Probe(ProbeContext context)
        {
        }

        public void Accept(StateMachineVisitor visitor)
        {
        }

        public async Task Execute(BehaviorContext<PaymentState, PaymentInitiatedEvent> context, Behavior<PaymentState, PaymentInitiatedEvent> next)
        {
            await _paymentAggregate.BeginPaymentProcessAsyncInternal(context.Data.Country, context.Data.Currency,
                context.Data.System, context.Data.ExternalId, context.Data.ExternalCallbackUrl, context.Data.Amount);

            await next.Execute(context);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<PaymentState, PaymentInitiatedEvent, TException> context, Behavior<PaymentState, PaymentInitiatedEvent> next) where TException : Exception
        {
            return next.Faulted(context);
        }
    }

    public class PaymentInitiatedEvent
    {
        public PaymentInitiatedEvent(string country, string currency, string system, decimal amount, string externalId, string externalCallbackUrl)
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

    public class PaymentStateMachine : AutomatonymousStateMachine<PaymentState>
    {
        public PaymentStateMachine(PaymentAggregate paymentAggregate)
        {
            InstanceState(x => x.MachineState);

            Initially(
                When(PaymentInitiated)
                    .Execute(context => new BeginPaymentProcess(paymentAggregate))
                    .TransitionTo(InProgress));

            During(InProgress,
                When(PaymentPinged)
                    .Execute(context => new PingPayment(paymentAggregate)),
                When(PaymentCancelled)
                    .Execute(context => new CancelPaymentProcess(paymentAggregate))
                    .TransitionTo(Cancelled)
                    .Finalize());
        }

        public Event<PaymentInitiatedEvent> PaymentInitiated { get; private set; }
        public Event PaymentCancelled { get; private set; }
        public Event PaymentPinged { get; private set; }

        public State InProgress { get; private set; }
        public State Cancelled { get; private set; }
    }
}