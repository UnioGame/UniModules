namespace UniModule.UnityTools.UniStateMachine.AsyncStateMachine
{
    public interface IAsyncStateFactory<TStateType>
    {
        IAsyncStateBehaviour Create(TStateType state);
    }
}