using System;
using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using Assets.Tools.UnityTools.UniRoutine;

namespace Assets.Tools.UnityTools.StateMachine.ContextStateMachine
{
    public class ContextStateExecutor : IContextStateExecutor<IEnumerator>
    {


        public IDisposable Execute(IContextStateBehaviour<IEnumerator> state, 
            IContext context)
        {
            var routine = state.Execute(context);
            var dispose = routine.RunWithSubRoutines();
            return dispose;
        }

        public void Dispose()
        {
            
        }
    }
}
