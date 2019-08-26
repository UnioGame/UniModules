namespace UniGreenModules.UniStateMachine.Runtime.AsyncStateMachine
{
    using Interfaces;
    using UniRx.Async;

    public interface IAsyncStateBehaviour : IStateBehaviour<UniTask>{}
    
}