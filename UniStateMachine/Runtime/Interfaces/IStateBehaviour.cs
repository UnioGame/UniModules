using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniRx;

    public interface IStateBehaviour<out TResult> : IRoutine<TResult>, IEndPoint
    {
        
        IReadOnlyReactiveProperty<bool> IsActive { get; }
        
    }

    public interface IStateBehaviour< in TContext,out TResult> :
        IRoutine<TContext,TResult>, 
        IEndPoint
    {
    }
    
}