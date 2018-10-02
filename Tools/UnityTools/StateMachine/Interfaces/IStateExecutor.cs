
namespace UniStateMachine
{
    public interface IStateExecutor<TState>
    {
        void Execute(TState state);
        void Stop();
    }

}


