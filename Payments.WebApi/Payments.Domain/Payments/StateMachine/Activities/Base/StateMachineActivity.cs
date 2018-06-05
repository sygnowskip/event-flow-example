using System;
using System.Threading.Tasks;
using Automatonymous;
using GreenPipes;

namespace Payments.Domain.Payments.StateMachine.Activities.Base
{
    public abstract class StateMachineActivity<TState, TData> : StateMachineActivityBase<TState>, Activity<TState, TData>
    {
        public abstract Task Execute(TData data);
        public async Task Execute(BehaviorContext<TState, TData> context, Behavior<TState, TData> next)
        {
            await Execute(context.Data);
            await next.Execute(context);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<TState, TData, TException> context, Behavior<TState, TData> next) where TException : Exception
        {
            return next.Faulted(context);
        }
    }

    public abstract class StateMachineActivity<TState> : StateMachineActivityBase<TState>, Activity<TState>
    {
        public abstract Task Execute();

        public async Task Execute(BehaviorContext<TState> context, Behavior<TState> next)
        {
            await Execute();
            await next.Execute(context);
        }

        public async Task Execute<T1>(BehaviorContext<TState, T1> context, Behavior<TState, T1> next)
        {
            await Execute();
            await next.Execute(context);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<TState, TException> context, Behavior<TState> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public Task Faulted<T1, TException>(BehaviorExceptionContext<TState, T1, TException> context, Behavior<TState, T1> next) where TException : Exception
        {
            return next.Faulted(context);
        }
    }

    public abstract class StateMachineActivityBase<TState> : Activity
    {
        public void Probe(ProbeContext context)
        {
            context.CreateScope(typeof(TState).Name);
        }

        public void Accept(StateMachineVisitor visitor) { }
    }
}