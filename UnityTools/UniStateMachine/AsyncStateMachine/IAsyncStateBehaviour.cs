using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniRx.Async;

namespace Assets.Tools.UnityTools.StateMachine.AsyncStateMachine
{
    public interface IAsyncStateBehaviour : IStateBehaviour<UniTask>{
    }
}