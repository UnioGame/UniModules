using System;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using StateMachine.ContextStateMachine;

namespace Assets.Modules.UnityToolsModule.Tools.UnityTools.StateMachine.Interfaces
{
    public interface IContextStateExecutor<TAwaiter>  :IDisposable
    {

        IDisposable Execute(IContextStateBehaviour<TAwaiter> state, IContext context);

    }
}
