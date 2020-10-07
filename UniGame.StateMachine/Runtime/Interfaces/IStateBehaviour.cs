namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;
    using UniModules.UniCore.Runtime.Interfaces;
    using UniRx;

    public interface IStateBehaviour<out TResult> : 
        IRoutine<TResult>, 
        IEndPoint
    {
        
        IReadOnlyReactiveProperty<bool> IsActive { get; }

    }

    public interface IStateBehaviour< in TContext,out TResult> :
        IRoutine<TContext,TResult>, 
        IEndPoint
    {
    }
    
}