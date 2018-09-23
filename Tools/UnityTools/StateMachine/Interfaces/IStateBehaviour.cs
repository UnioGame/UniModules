
using System;
using UniRx;

namespace UniStateMachine
{
    public interface IStateBehaviour<TResult> : IRoutine<TResult>
    {
        
        bool IsActive { get; }
        void Exit();
        
    }

    public interface IStateBehaviour<TContext,TResult> :IStateBehaviour<TResult>,IInitializable<TContext>
    {
        
        
        
    }
    
}