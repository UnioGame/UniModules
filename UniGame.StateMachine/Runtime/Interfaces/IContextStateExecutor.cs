namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    using System;
    using UniGame.Core.Runtime.Interfaces;

    public interface IContextStateExecutor<TAwaiter>  :IDisposable
    {

        IDisposable Execute(IContextState<TAwaiter> state, IContext context);

    }
}
