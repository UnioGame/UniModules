
using UniRx.Async;

namespace Assets.Scripts.Tools.StateMachine.AsyncStateMachine
{
    public interface IAsyncStateBehaviour : IStateBehaviour<UniTask>{
    }
}