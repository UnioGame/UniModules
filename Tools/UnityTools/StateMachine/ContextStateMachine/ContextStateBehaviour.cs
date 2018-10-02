using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ProfilerTools;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;

namespace StateMachine.ContextStateMachine
{
    public class ContextStateBehaviour : IContextStateBehaviour<IEnumerator>
    {
        protected Dictionary<IContextProvider, bool> _contextItems = new Dictionary<IContextProvider, bool>();

        #region public methods

        public IEnumerator Execute(IContextProvider context)
        {
            if (context == null)
            {
                GameLog.LogErrorFormat("NULL CONTEXT OF STATE {0}",GetType().Name);
                yield break;
            }

            if (IsActive(context))
                yield return Wait(context);

            AddContext(context);
            Initialize(context);

            yield return ExecuteState(context);

            OnPostExecute(context);
        }

        public bool IsActive(IContextProvider context)
        {
            return _contextItems.ContainsKey(context);
        }

        public void Exit(IContextProvider context)
        {
            RemoveContext(context);
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

        protected void RemoveContext(IContextProvider context)
        {
            _contextItems.Remove(context);
        }

        protected void AddContext(IContextProvider context)
        {
            _contextItems[context] = true;
        }

        protected virtual IEnumerator Wait(IContextProvider context)
        {
            while (IsActive(context))
            {
                yield return null;
            }
        }

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
