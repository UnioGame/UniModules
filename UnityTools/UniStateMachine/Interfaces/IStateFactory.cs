namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    public interface IStateFactory<TStateType, TResult>
    {
        TResult Create(TStateType state);
    }
}