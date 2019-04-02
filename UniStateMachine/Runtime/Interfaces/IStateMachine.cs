using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    public interface IStateMachine<TState> : 
        ICommonExecutor<TState>
    {

    }
}