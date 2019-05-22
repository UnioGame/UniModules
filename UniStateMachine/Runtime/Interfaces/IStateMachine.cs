using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public interface IStateMachine<TState> : 
        ICommonExecutor<TState>
    {

    }
}