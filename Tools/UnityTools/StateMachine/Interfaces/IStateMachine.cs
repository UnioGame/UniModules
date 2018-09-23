using System;
using Assets.Scripts.StateMachine;

namespace UniStateMachine
{
    public interface IStateMachine<TState> : 
        ICommonExecutor<TState>
    {

    }
}