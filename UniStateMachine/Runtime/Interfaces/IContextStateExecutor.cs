using System;
using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public interface IContextStateExecutor<TAwaiter>  :IDisposable
    {

        IDisposable Execute(IContextState<TAwaiter> state, IContext context);

    }
}
