using UniModule.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine.Interfaces
{
    public interface IStateMachine<TState> : 
        ICommonExecutor<TState>
    {

    }
}