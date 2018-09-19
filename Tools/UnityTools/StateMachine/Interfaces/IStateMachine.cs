using System;
using Assets.Scripts.StateMachine;

namespace Assets.Scripts.Tools.StateMachine
{
    public interface IStateMachine<TState> : 
        ICommonExecutor<TState>
    {

    }
}