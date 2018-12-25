
namespace Assets.Tools.UnityTools.StateMachine.Interfaces
{
    public interface IStateExecutor<TState>
    {
        void Execute(TState state);
        void Stop();
    }

}


