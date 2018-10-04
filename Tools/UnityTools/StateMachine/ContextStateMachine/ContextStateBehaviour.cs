using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine.ContextStateMachine
{
    public abstract class ContextStateBehaviour : IContextStateBehaviour<IEnumerator>
    {

        private bool _initialized = false;

        #region public methods

        public IEnumerator Execute(IContext context)
        {
            if (_initialized == false)
            {
                _initialized = true;
                OnInitialize();
            }

            yield return ExecuteState(context);

            OnPostExecute(context);
        }

        public void Exit(IContext context)
        {
            OnExit(context);
        }

        public abstract void Dispose();

        #endregion

        protected virtual void OnInitialize() { }

        protected virtual void OnExit(IContext context){}

        protected virtual void OnPostExecute(IContext context){}

        protected abstract IEnumerator ExecuteState(IContext context);

    }
}
