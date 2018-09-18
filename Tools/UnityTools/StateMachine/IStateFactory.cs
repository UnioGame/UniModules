namespace Assets.Scripts.Tools.StateMachine
{
    public interface IStateFactory<TStateType, TAwaiter>
    {
        IStateBehaviour<TAwaiter> Create(TStateType state);
    }
}