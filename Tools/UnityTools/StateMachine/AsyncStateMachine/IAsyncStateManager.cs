namespace UniStateMachine
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