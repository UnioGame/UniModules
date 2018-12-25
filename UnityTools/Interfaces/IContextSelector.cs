using Assets.Tools.UnityTools.StateMachine.Interfaces;

namespace Assets.Tools.UnityTools.Interfaces
{
    public interface IContextSelector<TResult>
    {
        IContextState<TResult> Select(IContext context);
    }
}
