namespace UniModules.UniStateMachine.Runtime.AsyncStateMachine.Interfaces
{
    using System;
    using Cysharp.Threading.Tasks;


    public interface IAsyncStateExecutor : IDisposable
    {
        void Execute(UniTask task);
        void Stop();
    }

}



