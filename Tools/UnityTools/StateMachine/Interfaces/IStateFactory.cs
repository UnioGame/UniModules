namespace Assets.Tools.UnityTools.StateMachine.Interfaces
{
    public interface IStateFactory<TStateType, TResult>
    {
        TResult Create(TStateType state);
    }
}