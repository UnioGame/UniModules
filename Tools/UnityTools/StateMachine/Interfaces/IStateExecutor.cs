using System;
using System.Collections;

namespace Assets.Scripts.Tools.StateMachine
{
    public interface IStateExecutor<TState>
    {
        void Execute(TState state);
        void Stop();
    }

}


