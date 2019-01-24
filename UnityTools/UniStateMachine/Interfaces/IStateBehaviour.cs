using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    public interface IStateBehaviour<out TResult> : IRoutine<TResult>
    {
        
        bool IsActive { get; }
        void Exit();
        
    }

    public interface IStateBehaviour< in TContext,out TResult> :
        IRoutine<TContext,TResult>
    {
        void Exit(TContext context);
    }
    
}