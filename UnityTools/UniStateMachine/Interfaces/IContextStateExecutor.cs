using System;
using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    public interface IContextStateExecutor<TAwaiter>  :IDisposable
    {

        IDisposable Execute(IContextState<TAwaiter> state, IContext context);

    }
}
