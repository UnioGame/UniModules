namespace Assets.Scripts.Tools.StateMachine.AsyncStateMachine
{
    public interface IAsyncStateFactory<TStateType>
    {
        IAsyncStateBehaviour Create(TStateType state);
    }
}