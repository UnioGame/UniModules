
using System;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
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