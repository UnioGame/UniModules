using UniModule.UnityTools.UniStateMachine.Interfaces;

namespace UniModule.UnityTools.UniStateMachine.AsyncStateMachine
{
    public interface IAsyncStateManager<TStateType>
    {
        TStateType CurrentState { get; }
        TStateType PreviousState { get; }

        IStateController<TStateType> StateController { get; }

        void Dispose();
        void SetState(TStateType state);
    }
}