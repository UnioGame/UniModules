namespace UniGreenModules.UniStateMachine.Runtime.AsyncStateMachine.Interfaces
{
    using Runtime.Interfaces;
    using UniRx.Async;

    public interface IAsyncStateBehaviour : IStateBehaviour<UniTask>{}
    
}