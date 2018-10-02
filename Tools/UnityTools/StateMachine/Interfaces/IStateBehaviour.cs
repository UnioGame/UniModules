using System;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;

namespace UniStateMachine
{
    public interface IStateBehaviour<out TResult> : IRoutine<TResult>
    {
        
        bool IsActive { get; }
        void Exit();
        
    }

    public interface IStateBehaviour< in TContext,out TResult> :
        IRoutine<TContext,TResult>
    {
        bool IsActive(TContext context);
        void Exit(TContext context);
    }
    
}