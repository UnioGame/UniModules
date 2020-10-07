namespace UniModules.UniStateMachine.Runtime.AsyncStateMachine.Interfaces
{
    using Runtime.Interfaces;

    public interface IAsyncStateManager<TStateType>
    {
        TStateType CurrentState { get; }
        TStateType PreviousState { get; }

        IStateController<TStateType> StateController { get; }

        void Dispose();
        void SetState(TStateType state);
    }
}