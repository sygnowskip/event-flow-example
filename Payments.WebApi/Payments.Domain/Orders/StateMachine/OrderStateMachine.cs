using System.Threading.Tasks;
using Automatonymous;
using Payments.Domain.Orders.StateMachine.Models;

namespace Payments.Domain.Orders.StateMachine
{
    public class OrderStateMachine : AutomatonymousStateMachine<OrderAggregate>
    {
        public OrderStateMachine()
        {
            InstanceState(aggregate => aggregate.StateMachineState, Created, PaymentProcessStarted, Completed);

            Initially(
                When(OrderCreationRequested)
                    .ThenAsync(CreateOrder)
                    .TransitionTo(Created));

            During(Created,
                When(AddProductToOrderRequested)
                    .ThenAsync(AddProductToOrder),
                When(InitPaymentProcessRequested)
                    .ThenAsync(InitPaymentProcess)
                    .TransitionTo(PaymentProcessStarted));

            During(PaymentProcessStarted,
                When(PaymentProcessCanceled)
                    .ThenAsync(MarkPaymentProcessAsFailed)
                    .TransitionTo(Created),
                When(PaymentProcessSuccessfullyFinished)
                    .ThenAsync(CompletePaymentProcess)
                    .TransitionTo(Completed));
        }

        private Task MarkPaymentProcessAsFailed(BehaviorContext<OrderAggregate> context)
        {
            context.Instance.MarkPaymentProcessAsFailed();
            return Task.CompletedTask;
        }

        private Task CompletePaymentProcess(BehaviorContext<OrderAggregate> context)
        {
            context.Instance.CompletePaymentProcess();
            return Task.CompletedTask;
        }

        private Task InitPaymentProcess(BehaviorContext<OrderAggregate> context)
        {
            context.Instance.BeginPaymentProcess();
            return Task.CompletedTask;
        }

        private Task AddProductToOrder(BehaviorContext<OrderAggregate, AddProductToOrderData> context)
        {
            context.Instance.AddProductToOrder(context.Data.Name, context.Data.Count, context.Data.Price);
            return Task.CompletedTask;
        }

        private Task CreateOrder(BehaviorContext<OrderAggregate, string> context)
        {
            context.Instance.CreateOrder(context.Data);
            return Task.CompletedTask;
        }



        public Event<string> OrderCreationRequested { get; private set; }
        public Event<AddProductToOrderData> AddProductToOrderRequested { get; private set; }
        public Event InitPaymentProcessRequested { get; private set; }
        public Event PaymentProcessSuccessfullyFinished { get; private set; }
        public Event PaymentProcessCanceled { get; private set; }


        public State Created { get; private set; }
        public State PaymentProcessStarted { get; private set; }
        public State Completed { get; private set; }
    }
}