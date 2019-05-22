using UniModule.UnityTools.UniStateMachine.Interfaces;

namespace UniModule.UnityTools.Interfaces
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public interface IContextSelector<TResult> : 
        ISelector<IContext,IContextState<TResult>>
    {
    }
}
