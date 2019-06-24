namespace UniGreenModules.UniStateMachine.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;

    public interface IContextSelector<TResult> : 
        ISelector<IContext,IContextState<TResult>>
    {
    }
}
