namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    using global::UniGame.Core.Runtime;

    public interface IContextSelector<TResult> : 
        ISelector<IContext,IContextState<TResult>>
    {
    }
}
