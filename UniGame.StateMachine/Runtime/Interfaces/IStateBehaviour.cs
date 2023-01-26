namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    using global::UniGame.Core.Runtime;
    using UniRx;

    public interface IStateBehaviour<TResult> : 
        IRoutine<TResult>, 
        IEndPoint
    {
        
        IReadOnlyReactiveProperty<bool> IsActive { get; }

    }

    public interface IStateBehaviour<TContext,TResult> :
        IRoutine<TContext,TResult>, 
        IEndPoint
    {
    }
    
}