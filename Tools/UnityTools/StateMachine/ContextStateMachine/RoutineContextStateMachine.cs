using System.Collections;
using Assets.Tools.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine.ContextStateMachine
{
    public class RoutineContextStateMachine : ContextStateMachine<IEnumerator>
    {
        public RoutineContextStateMachine(IContextExecutor<IEnumerator> stateExecutor) : 
            base(stateExecutor)
        {
        }
    }
}
