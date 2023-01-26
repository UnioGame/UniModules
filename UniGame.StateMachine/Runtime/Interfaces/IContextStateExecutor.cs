namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    using System;
    using global::UniGame.Core.Runtime;

    public interface IContextStateExecutor<TAwaiter>  :IDisposable
    {

        IDisposable Execute(IContextState<TAwaiter> state, IContext context);

    }
}
