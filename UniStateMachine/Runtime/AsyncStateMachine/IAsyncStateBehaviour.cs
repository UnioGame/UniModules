using UniModule.UnityTools.UniStateMachine.Interfaces;
using UniRx.Async;

namespace UniModule.UnityTools.UniStateMachine.AsyncStateMachine
{
    public interface IAsyncStateBehaviour : IStateBehaviour<UniTask>{}
    
}