using System;
using System.Collections;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.Interfaces;

namespace Modules.UnityToolsModule.Tools.UnityTools.Interfaces
{
    public interface IExecutor : IRoutineExecutor<IEnumerator>{
    }

    public interface IRoutineExecutor<TAwaiter>: 
        ICommonExecutor<TAwaiter>
    {
        bool IsActive { get; }
    }

    public interface IProcess
    {
        bool IsActive { get; }
        void Execute();
        void Stop();
    }
    
    public interface ICommonExecutor<TData>
    {
        void Execute(TData data);
        void Stop();
    }

    public interface IContextExecutor<TContext>
    {
        IDisposableItem Execute(TContext context);
    }
}