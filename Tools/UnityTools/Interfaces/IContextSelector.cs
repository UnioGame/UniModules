using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using StateMachine.ContextStateMachine;
using UniStateMachine;

namespace Assets.Modules.UnityToolsModule
{
    public interface IContextSelector<TResult>
    {
        IContextStateBehaviour<TResult> Select(IContextProvider context);
    }
}
