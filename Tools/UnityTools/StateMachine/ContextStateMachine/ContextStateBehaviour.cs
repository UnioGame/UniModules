using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ProfilerTools;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;

namespace StateMachine.ContextStateMachine
{
    public class ContextStateBehaviour : IContextStateBehaviour<IEnumerator>
    {

        #region public methods

        public IEnumerator Execute(IContextProvider context)
        {
            Initialize(context);

            yield return ExecuteState(context);

            OnPostExecute(context);
        }

        public void Exit(IContextProvider context)
        {
            OnExit(context);
        }

        public void Dispose()
        {
            foreach (var context in _contextItems)
            {
                Exit(context.Key);
            }
            _contextItems.Clear();
        }

        #endregion

        protected virtual void OnExit(IContextProvider context)
        {

        }

        protected virtual void Initialize(IContextProvider context)
        {
        }

        protected virtual void OnPostExecute(IContextProvider context)
        {

        }

        protected virtual IEnumerator ExecuteState(IContextProvider context)
        {
            yield break;
        }


    }
}
