
namespace UniModules.UniStateMachine.Runtime.Interfaces
{
    public interface IStateExecutor<TState>
    {
        void Execute(TState state);
        void Stop();
    }

}


