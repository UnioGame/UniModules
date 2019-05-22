using System;
using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public interface IContextStateMachine<TAwaiter> : IDisposable
    {
        bool IsActive { get; }

        IContext Context { get; }
        IContextState<TAwaiter> ActiveState { get; }
        void Execute(IContextState<TAwaiter> state, IContext context);
        void Stop();

    }
}
