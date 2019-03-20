using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    public interface IStateBehaviour<out TResult> : IRoutine<TResult>, IEndPoint
    {
        
        bool IsActive { get; }
    }

    public interface IStateBehaviour< in TContext,out TResult> :
        IRoutine<TContext,TResult>, 
        IEndPoint
    {
    }
    
}