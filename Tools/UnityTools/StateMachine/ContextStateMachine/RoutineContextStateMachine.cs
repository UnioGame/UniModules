using System;
using System.Collections;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;

namespace UniStateMachine
{
    public class RoutineContextStateMachine : ContextStateMachine<IEnumerator>
    {
        public RoutineContextStateMachine(IRoutineExecutor<IEnumerator> stateExecutor) : base(stateExecutor)
        {
        }
    }
}
