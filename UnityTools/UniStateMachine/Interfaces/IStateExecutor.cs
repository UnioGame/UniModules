
namespace UniModule.UnityTools.UniStateMachine.Interfaces
{
    public interface IStateExecutor<TState>
    {
        void Execute(TState state);
        void Stop();
    }

}


