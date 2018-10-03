using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine.ContextStateMachine
{
    public class ContextStateBehaviour : IContextStateBehaviour<IEnumerator>
    {

        #region public methods

        public IEnumerator Execute(IContext context)
        {
            Initialize(context);

            yield return ExecuteState(context);

            OnPostExecute(context);
        }

        public void Exit(IContext context)
        {
            OnExit(context);
        }

        public virtual void Dispose()
        {

        }

        #endregion

        protected virtual void OnExit(IContext context)
        {

        }

        protected virtual void Initialize(IContext context)
        {
        }

        protected virtual void OnPostExecute(IContext context)
        {

        }

        protected virtual IEnumerator ExecuteState(IContext context)
        {
            yield break;
        }


    }
}
