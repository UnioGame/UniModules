namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    using UniGame.Core.Runtime.Interfaces;

    public interface IStateMachine<TState> : 
        ICommonExecutor<TState>
    {

    }
}