using System;
using Assets.Tools.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine.Interfaces
{
    public interface IContextStateExecutor<TAwaiter>  :IDisposable
    {

        IDisposable Execute(IContextStateBehaviour<TAwaiter> state, IContext context);

    }
}
