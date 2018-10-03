using System;
using System.Collections;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;

namespace UniStateMachine
{
    public class RoutineContextStateMachine : ContextStateMachine<IEnumerator>
    {
        public RoutineContextStateMachine(IContextExecutor<IEnumerator> stateExecutor) : 
            base(stateExecutor)
        {
        }
    }
}
