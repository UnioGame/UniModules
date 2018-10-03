using System;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UniStateMachine;

namespace StateMachine.ContextStateMachine
{
    public interface IContextStateMachine<TAwaiter> : IDisposable
    {

        IContextProvider Context { get; }

        IContextStateBehaviour<TAwaiter> ActiveState { get; }
        void Execute(IContextStateBehaviour<TAwaiter> state, IContextProvider context);
        void Stop();

    }
}
