namespace UniModules.UniStateMachine.Runtime.ContextStateMachine
{
    using System;
    using System.Collections;
    using Interfaces;
    using global::UniGame.Core.Runtime;
    using UniRoutine.Runtime;

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
