using System;
using Assets.Tools.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine.Interfaces
{
    public interface IContextStateMachine<TAwaiter> : IDisposable
    {
        bool IsActive { get; }

        IContext Context { get; }
        IContextState<TAwaiter> ActiveState { get; }
        void Execute(IContextState<TAwaiter> state, IContext context);
        void Stop();

    }
}
