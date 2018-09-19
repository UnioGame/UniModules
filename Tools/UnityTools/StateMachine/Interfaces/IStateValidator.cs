namespace Assets.Scripts.Tools.StateMachine
{
    public interface IStateValidator<TStateType>  {

        bool Validate(TStateType fromState, TStateType toState);
    }
}
