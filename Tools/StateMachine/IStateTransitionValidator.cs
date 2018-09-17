namespace Assets.Scripts.Tools.StateMachine
{
    public interface IStateTransitionValidator<TStateType>  {

        bool Validate(TStateType from, TStateType to);
    }
}
