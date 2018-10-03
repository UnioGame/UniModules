using System;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UniStateMachine;

namespace StateMachine.ContextStateMachine
{
    public interface IContextStateMachine<TAwaiter> : IDisposable
    {
        bool IsActive { get; }

        IContext Context { get; }
        IContextStateBehaviour<TAwaiter> ActiveState { get; }
        void Execute(IContextStateBehaviour<TAwaiter> state, IContext context);
        void Stop();

    }
}
