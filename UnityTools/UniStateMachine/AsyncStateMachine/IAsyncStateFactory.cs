namespace Assets.Tools.UnityTools.StateMachine.AsyncStateMachine
{
    public interface IAsyncStateFactory<TStateType>
    {
        IAsyncStateBehaviour Create(TStateType state);
    }
}