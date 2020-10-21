namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    using UniGame.Core.Runtime.Interfaces;

    public interface IContextSelector<TResult> : 
        ISelector<IContext,IContextState<TResult>>
    {
    }
}
