using System;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UniStateMachine;

namespace StateMachine.ContextStateMachine
{
    public interface IContextStateMachine<TAwaiter> : IDisposable
    {
        IContextStateBehaviour<TAwaiter> ActiveState { get; }
        bool IsActive { get; }

        void Execute(IContextStateBehaviour<TAwaiter> state, IContextProvider context);
        void Stop();

    }
}
