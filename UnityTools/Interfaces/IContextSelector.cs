using Assets.Tools.UnityTools.StateMachine.Interfaces;

namespace UniModule.UnityTools.Interfaces
{
    public interface IContextSelector<TResult>
    {
        IContextState<TResult> Select(IContext context);
    }
}
