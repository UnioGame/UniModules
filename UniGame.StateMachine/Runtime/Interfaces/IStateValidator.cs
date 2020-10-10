namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    public interface IStateValidator<TStateType>  {

        bool Validate(TStateType fromState, TStateType toState);
    }
}
