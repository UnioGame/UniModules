namespace UniGreenModules.UniStateMachine.Runtime.AsyncStateMachine
{
    public interface IAsyncStateFactory<TStateType>
    {
        IAsyncStateBehaviour Create(TStateType state);
    }
}