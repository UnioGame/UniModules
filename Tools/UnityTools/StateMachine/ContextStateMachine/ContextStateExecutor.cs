using System;
using System.Collections;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.StateMachine.Interfaces;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using StateMachine.ContextStateMachine;
using UnityToolsModule.Tools.UnityTools.UniRoutine;

namespace UnityTools.StateMachine.ContextStateMachine
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
