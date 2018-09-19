using System;
using System.Collections;

namespace Assets.Scripts.Tools.StateMachine
{
    public interface IStateExecutor<TState> : IDisposable
    {
        void Execute(TState state);
        void Stop();
    }

}


