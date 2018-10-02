using System;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UniStateMachine;

namespace StateMachine.ContextStateMachine
{
    public interface IContextStateBehaviour<out TAwaiter> : 
        IStateBehaviour<IContextProvider, TAwaiter>,
        IDisposable
    {
    }
}
