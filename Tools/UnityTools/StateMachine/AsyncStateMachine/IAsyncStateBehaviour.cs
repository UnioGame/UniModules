
using UniRx.Async;
using UniStateMachine;

namespace UniStateMachine
{
    public interface IAsyncStateBehaviour : IStateBehaviour<UniTask>{
    }
}