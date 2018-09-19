namespace Assets.Scripts.Tools.StateMachine
{
    public interface IStateValidator<TStateType>  {

        bool Validate(TStateType from, TStateType to);
    }
}
