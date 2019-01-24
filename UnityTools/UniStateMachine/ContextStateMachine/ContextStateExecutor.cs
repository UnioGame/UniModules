using System;
using System.Collections;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniRoutine;

namespace Assets.Tools.UnityTools.StateMachine.ContextStateMachine
{
    public class ContextStateExecutor : IContextStateExecutor<IEnumerator>
    {


        public IDisposable Execute(IContextState<IEnumerator> state, 
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
