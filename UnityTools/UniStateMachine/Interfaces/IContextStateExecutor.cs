using System;
using UniModule.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine.Interfaces
{
    public interface IContextStateExecutor<TAwaiter>  :IDisposable
    {

        IDisposable Execute(IContextState<TAwaiter> state, IContext context);

    }
}
