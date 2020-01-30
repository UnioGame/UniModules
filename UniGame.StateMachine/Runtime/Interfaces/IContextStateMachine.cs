namespace UniGreenModules.UniStateMachine.Runtime.Interfaces
{
    using System;
    using UniCore.Runtime.Interfaces;

    public interface IContextStateMachine<TAwaiter> : IDisposable
    {
        bool IsActive { get; }

        IContext Context { get; }
        IContextState<TAwaiter> ActiveState { get; }
        void Execute(IContextState<TAwaiter> state, IContext context);
        void Stop();

    }
}
