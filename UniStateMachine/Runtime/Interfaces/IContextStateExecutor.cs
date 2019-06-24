namespace UniGreenModules.UniStateMachine.Runtime.Interfaces
{
    using System;
    using UniCore.Runtime.Interfaces;

    public interface IContextStateExecutor<TAwaiter>  :IDisposable
    {

        IDisposable Execute(IContextState<TAwaiter> state, IContext context);

    }
}
