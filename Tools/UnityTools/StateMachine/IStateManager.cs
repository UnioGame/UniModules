namespace Assets.Scripts.Tools.StateMachine
{
    public interface IStateManager<TStateType>
    {
        TStateType CurrentState { get; }
        TStateType PreviousState { get; }
        void Dispose();
        void SetState(TStateType state);
    }
}