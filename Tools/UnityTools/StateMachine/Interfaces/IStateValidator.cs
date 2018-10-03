namespace Assets.Tools.UnityTools.StateMachine.Interfaces
{
    public interface IStateValidator<TStateType>  {

        bool Validate(TStateType fromState, TStateType toState);
    }
}
