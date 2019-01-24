using UniModule.UnityTools.UniStateMachine.Interfaces;

namespace UniModule.UnityTools.Interfaces
{
    public interface IContextSelector<TResult>
    {
        IContextState<TResult> Select(IContext context);
    }
}
