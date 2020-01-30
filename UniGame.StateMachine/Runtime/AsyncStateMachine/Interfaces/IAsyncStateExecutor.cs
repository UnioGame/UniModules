namespace UniGreenModules.UniStateMachine.Runtime.AsyncStateMachine.Interfaces
{
    using System;
    using UniRx.Async;

    public interface IAsyncStateExecutor : IDisposable
    {
        void Execute(UniTask task);
        void Stop();
    }

}



