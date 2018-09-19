namespace Assets.Scripts.Tools.StateMachine
{
    public interface IStateFactory<TStateType, TResult>
    {
        TResult Create(TStateType state);
    }
}