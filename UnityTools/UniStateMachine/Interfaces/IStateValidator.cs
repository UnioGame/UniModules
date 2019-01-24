namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    public interface IStateValidator<TStateType>  {

        bool Validate(TStateType fromState, TStateType toState);
    }
}
