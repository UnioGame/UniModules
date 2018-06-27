using System.Collections;

namespace Assets.Scripts.StateMachine
{
    public interface IExecutor
    {
        void Execute(IEnumerator enumerator);
        void Stop();
    }
}