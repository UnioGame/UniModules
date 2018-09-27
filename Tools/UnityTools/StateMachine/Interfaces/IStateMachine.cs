using System;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;

namespace UniStateMachine
{
    public interface IStateMachine<TState> : 
        ICommonExecutor<TState>
    {

    }
}