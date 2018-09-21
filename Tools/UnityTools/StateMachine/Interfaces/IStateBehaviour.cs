
using System;
using UniRx;

namespace Assets.Scripts.Tools.StateMachine
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