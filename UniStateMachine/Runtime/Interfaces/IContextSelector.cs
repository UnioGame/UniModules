using UniModule.UnityTools.UniStateMachine.Interfaces;

namespace UniModule.UnityTools.Interfaces
{
    public interface IContextSelector<TResult> : 
        ISelector<IContext,IContextState<TResult>>
    {
    }
}
