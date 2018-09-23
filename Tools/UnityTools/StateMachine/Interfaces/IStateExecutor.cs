using System;
using System.Collections;

namespace UniStateMachine
{
    public interface IStateExecutor<TState>
    {
        void Execute(TState state);
        void Stop();
    }

}


