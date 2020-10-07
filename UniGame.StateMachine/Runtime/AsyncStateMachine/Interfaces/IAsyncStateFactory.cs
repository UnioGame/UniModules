namespace UniModules.UniStateMachine.Runtime.AsyncStateMachine.Interfaces
{
    public interface IAsyncStateFactory<TStateType>
    {
        IAsyncStateBehaviour Create(TStateType state);
    }
}